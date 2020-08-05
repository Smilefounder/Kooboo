$(function () {
  var SITE_ID_QUERY_STRING = "?SiteId=" + Kooboo.getQueryString("SiteId");
  var typeId = Kooboo.getQueryString("type") || Kooboo.Guid.Empty;
  var initTimes = 0;
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        productId: Kooboo.getQueryString("id") || Kooboo.Guid.Empty,
        fields: [],
        startValidating: false,
        validationPassed: false,
        contentValues: {},
        siteLangs: {},
        categories: [],
        selectedCategories: [],
        multipleMedia: false,
        mediaDialogData: {},
        variants: [],
        specNames: [],
        fixedSpecFields: [],
        dynaSpecFields: [],
        dynamicSpecFields: [],
        typesMatrix: [],
        typesUrl: Kooboo.Route.Promotion.ListPage,
        showCategoriesModal: false,
        model: {
          name: "",
          isActive: false,
          canCombine: false,
          activeBasedOnDates: false,
          startDate: moment().format("YYYY-MM-DD HH:mm"),
          endDate: moment().format("YYYY-MM-DD HH:mm"),
          ruleType: "",
          promotionMethod: "",
          promotionTarget: "",
          priceAmountReached: "",
          amount: "",
          percent: "",
        },
        ruleTypes: [],
        promotionMethods: [],
        promotionTargets: [],
      };
    },
    mounted: function () {
      $.when(
        Kooboo.Site.Langs(),
        Kooboo.PromotionRule.getEdit()
      ).then(function (r1, r2) {
        var langRes = r1[0];
        promotionRules = r2[0];
        if (langRes.success && promotionRules.success) {
          self.siteLangs = langRes.model;
          self.ruleTypes = promotionRules.model.promotionRuleTypes || [];
          self.promotionMethods = promotionRules.model.promotionMethods || [];
          self.promotionTargets = promotionRules.model.promotionTargets || [];
        }
      });
    },
    methods: {
      dynamicFieldsChange: function (fields) {
        self.dynamicSpecFields = fields;
        fields.forEach(function (f) {
          if (f.options.length) {
            if (self.specNames.indexOf(f.name) == -1) {
              self.specNames.push(f.name);
            }
          } else {
            self.specNames = _.without(self.specNames, f.name);
          }
        });
        self.renderTable();
      },
      renderTable: function () {
        if (self.variants.length) {
          if (initTimes < 1) {
            self.typesMatrix = self.variants.map(function (vari) {
              var types = [];
              self.specNames.forEach(function (name) {
                var item = {
                  name: name,
                };

                var value = null;
                var keys = Object.keys(vari.variants);
                keys.forEach(function (key) {
                  if (key.toLowerCase() == name.toLowerCase()) {
                    value = vari.variants[key];
                  }
                });
                item.value = value;
                types.push(item);
              });

              var images = [];
              if (vari.images && vari.images.length) {
                images = vari.images.map(function (img) {
                  return {
                    url: img,
                    thumbnail: "/_thumbnail/80/80" + img + SITE_ID_QUERY_STRING,
                  };
                });
              }

              return {
                types: types,
                stock: vari.stock,
                price: vari.price,
                sku: vari.sku,
                skuImage: vari.thumbnail,
                skuThumbnail:
                  "/_thumbnail/80/80" + vari.thumbnail + SITE_ID_QUERY_STRING,
                images: images,
                online: vari.online,
                error: {
                  stock: false,
                  price: false,
                },
              };
            });
            initTimes++;
          } else {
            self.getTypeMatrix();
          }
        } else {
          self.getTypeMatrix();
        }
      },
      getTypeMatrix: function () {
        var types = [];
        self.fixedSpecFields.forEach(function (f) {
          var options = JSON.parse(f.selectionOptions).map(function (opt) {
            return {
              name: f.name,
              value: opt.key,
            };
          });

          types.push(options);
        });

        self.dynamicSpecFields.forEach(function (f) {
          if (f.options.length) {
            var options = f.options.map(function (opt) {
              return {
                name: f.name,
                value: opt,
              };
            });
            types.push(options);
          }
        });

        var matrix = getTableDataByTypes(types);
        self.typesMatrix = matrix.map(function (m) {
          var find = _.find(self.typesMatrix, function (row) {
            return getValue(row.types) == getValue(m);

            function getValue(list) {
              return list
                .map(function (item) {
                  return item.value;
                })
                .join(",");
            }
          });

          return (
            find || {
              types: m,
              stock: "",
              price: "",
              sku: "",
              skuImage: "",
              skuThumbnail: "",
              images: [],
              online: true,
              error: {
                stock: false,
                price: false,
              },
            }
          );
        });
      },
      removeSkuPic: function (m) {
        m.skuImage = "";
        m.skuThumbnail = "";
      },
      selectSkuPic: function (m) {
        self.multipleMedia = false;
        Kooboo.Media.getList().then(function (res) {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = m;
            res.model["onAdd"] = function (selected) {
              m.skuImage = selected.url;
              m.skuThumbnail = selected.thumbnail;
            };
            self.mediaDialogData = res.model;
          }
        });
      },
      selectImages: function (m) {
        self.multipleMedia = true;
        Kooboo.Media.getList().then(function (res) {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = m;
            res.model["onAdd"] = function (selected) {
              m.images = selected.map(function (s) {
                return {
                  url: s.url,
                  thumbnail: s.thumbnail,
                };
              });
            };
            self.mediaDialogData = res.model;
          }
        });
      },
      removeImg: function (m, index) {
        m.images.splice(index, 1);
      },
      onSaveAndReturn: function () {
        self.onSave(function () {
          location.href = Kooboo.Route.Product.ListPage;
        });
      },
      onSave: function (cb) {
        if (self.isValid()) {
          var variants = self.typesMatrix.map(function (row) {
              var specs = {};
              if (row.types && row.types.length) {
                row.types.forEach(function (t) {
                  specs[t.name] = t.value;
                });
              }
              return {
                variants: specs,
                stock: row.stock,
                price: row.price,
                sku: row.sku,
                thumbnail: row.skuImage,
                images: row.images.map(function (img) {
                  return img.url;
                }),
                online: row.online,
              };
            }),
            categories = self.selectedCategories.map(function (cate) {
              return cate.id;
            });
          Kooboo.Product.post({
            id: self.productId,
            type: typeId,
            values: self.contentValues.fieldsValue,
            variants: variants,
            categories: categories,
          }).then(function (res) {
            if (res.success) {
              if (cb && typeof cb == "function") {
                cb();
              } else {
                location.href = Kooboo.Route.Get(
                  Kooboo.Route.Product.DetailPage,
                  {
                    id: res.model,
                    type: typeId,
                  }
                );
              }
            }
          });
        }
      },
      isValid: function () {
        var valid = self.$refs.fieldPanel.validate();
        if (!valid) return;

        self.typesMatrix.forEach(function (row) {
          if (
            self.validateInput(row, "price") ||
            self.validateInput(row, "stock")
          ) {
            valid = false;
          }
        });

        return valid;
      },
      getCategories: function (cates, selectedIds) {
        var temp = [];
        cates.forEach(function (c) {
          c.selected = selectedIds.indexOf(c.id) > -1;
          if (c.subCats && c.subCats.length) {
            c.subCats = self.getCategories(c.subCats, selectedIds);
          }
          temp.push(c);
        });

        return temp;
      },
      toggleStatus: function (m) {
        m.online = !m.online;
      },
      onShowCategoriesModal: function () {
        self.showCategoriesModal = true;
      },
      onHideCategoriesModal: function () {
        self.showCategoriesModal = false;
      },
      onSaveCategoriesModal: function () {
        self.selectedCategories = getSelected(self.categories);
        self.onHideCategoriesModal();
      },
      validateInput: function (data, fieldName) {
        return (data.error[fieldName] = data[fieldName] === "");
      },
    },
    computed: {
      isNew: function () {
        return self.productId == Kooboo.Guid.Empty;
      },
    },
  });

  function getSelected(cates) {
    var temp = [];
    cates.forEach(function (c) {
      if (c.selected) {
        temp.push(c);
      }

      if (c.subCats && c.subCats.length) {
        temp = _.concat(temp, getSelected(c.subCats));
      }
    });
    return temp;
  }

  function getTableDataByTypes() {
    return Array.prototype.reduce.call(
      arguments[0],
      function (a, b) {
        var ret = [];
        a.forEach(function (a) {
          b.forEach(function (b) {
            ret.push(a.concat([b]));
          });
        });
        return ret;
      },
      [[]]
    );
  }

  Vue.component("product-category", {
    template: "#category-template",
    props: {
      category: Object,
    },
  });
});
