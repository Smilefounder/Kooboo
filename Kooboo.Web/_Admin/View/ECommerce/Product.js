$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        categoryId: Kooboo.getQueryString("category"),
        id: Kooboo.getQueryString("id"),
        mediaDialogData: {},
        title: "",
        images: [],
        description: "",
        attributes: [],
        specifications: [],
        richeditor: {
          mediaDialogData: null,
          editorConfig: {
            min_height: 300,
            max_height: 600,
          },
        },
        productsUrl: Kooboo.Route.Product.ListPage,
        skus: [],
      };
    },
    mounted: function () {
      if (self.id) {
        $.when(
          Kooboo.ProductCategory.Get({
            id: self.categoryId,
          }),
          Kooboo.Product.Get({ id: self.id })
        ).then(self.initData);
      } else {
        self.id = Kooboo.Guid.NewGuid();

        $.when(
          Kooboo.ProductCategory.Get({
            id: self.categoryId,
          })
        ).then(self.initData);
      }

      Kooboo.EventBus.subscribe("ko/style/list/pickimage/show", function (ctx) {
        Kooboo.Media.getList().then(function (res) {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = ctx;
            res.model["onAdd"] = function (selected) {
              self.images.push({
                url: selected.url,
                isPrimary: self.images.length == 0,
              });
            };
            self.mediaDialogData = res.model;
          }
        });
      });
    },
    methods: {
      rmoveItem: function (items, item) {
        var index = items.indexOf(item);
        items.splice(index, 1);
        self.rebuildSkus();
      },
      addOption: function (item) {
        if (!item.editingItem) return;

        item.options.push({
          id: Kooboo.Guid.computeGuid(self.id + item.id + item.editingItem),
          value: item.editingItem,
        });

        item.editingItem = "";
        self.rebuildSkus();
      },
      switchOption: function (option) {
        option.checked = !option.checked;
        self.rebuildSkus();
      },
      showMediaDialog: function () {
        Kooboo.EventBus.publish("ko/style/list/pickimage/show");
      },
      save: function () {
        var model = {
          id: self.id,
          title: self.title,
          images: JSON.stringify(self.images),
          description: self.description,
          attributes: JSON.stringify(
            self.attributes.map(function (m) {
              return {
                id: m.id,
                name: m.name,
                value: m.value,
              };
            })
          ),
          categoryId: self.categoryId,
        };

        var skus = JSON.parse(JSON.stringify(self.skus));

        skus.forEach(function (f) {
          f.stock = f.stock - f.lastStock;
          f.specifications = JSON.stringify(f.specifications);
        });

        model.skus = skus;

        Kooboo.Product.post(model).then(function (res) {
          if (res.success) {
            window.info.show(Kooboo.text.info.save.success, true);
          }
        });
      },
      saveAndReturn: function () {
        self.save();
        location.href = self.productsUrl;
      },
      rebuildSkus: function () {
        var skus = [];
        function recursion(specification, item) {
          var options = self.getOptions(specification);
          var next =
            self.specifications[self.specifications.indexOf(specification) + 1];

          options.forEach(function (o) {
            var coped = JSON.parse(JSON.stringify(item));

            coped.specifications.push({
              id: o.id,
              name: specification.name,
              value: o.value,
            });

            if (next) {
              recursion(next, coped);
            } else {
              var id = Kooboo.Guid.computeGuid(
                self.id +
                  coped.specifications
                    .map(function (m) {
                      return m.id;
                    })
                    .join("")
              );

              var sku =
                self.skus.filter(function (f) {
                  return f.id == id;
                })[0] || coped;

              sku.id = id;
              skus.push(sku);
            }
          });
        }

        recursion(self.specifications[0], {
          specifications: [],
          price: 0,
          tax: 0,
          enable: true,
          name: "",
          productId: self.id,
          stock: 0,
          lastStock: 0,
        });

        self.skus = skus;
      },
      getOptions: function (specification) {
        if (specification.type == 0) return specification.options;
        else {
          return specification.options.filter(function (f) {
            return f.checked;
          });
        }
      },
      initData: function (category, product) {
        var isNew = typeof product == "string";

        if (isNew) {
          product = null;
        } else {
          category = category[0];
          product = product[0];
          self.title = product.model.title;
          self.images = JSON.parse(product.model.images);
          self.description = product.model.description;
        }
        var attributes = JSON.parse(category.model.attributes);
        var specifications = JSON.parse(category.model.specifications);
        var productAttributes = product
          ? JSON.parse(product.model.attributes)
          : [];

        attributes.forEach(function (f) {
          var productAttribute = productAttributes.filter(function (a) {
            return a.id == f.id;
          })[0];

          f.value = productAttribute ? productAttribute.value : "";
        });

        specifications.forEach(function (f) {
          if (f.type == 0) f.editingItem = "";

          if (f.type == 1) {
            f.options.forEach(function (o) {
              o.checked = false;
            });
          }
        });

        self.specifications = specifications;
        self.attributes = attributes;
      },
      imgUrlWrap: function (url) {
        return "url('" + url + self.querySiteId + "')";
      },
      setIsPrimary: function (item) {
        this.images.forEach(function (f) {
          f.isPrimary = false;
        });

        item.isPrimary = true;
      },
      removeImage: function (item) {
        self.images = self.images.filter(function (f) {
          return f != item;
        });

        if (self.images.length && item.isPrimary) {
          self.images[0].isPrimary = true;
        }
      },
    },
  });
});
