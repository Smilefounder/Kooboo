$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        typeId: Kooboo.getQueryString("type"),
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
        enable: true,
        categories: [],
      };
    },
    mounted() {
      if (self.id) {
        $.when(
          Kooboo.ProductType.Get({
            id: self.typeId,
          }),
          Kooboo.Product.Get({ id: self.id })
        ).then(self.initData);
      } else {
        self.id = Kooboo.Guid.NewGuid();

        $.when(
          Kooboo.ProductType.Get({
            id: this.typeId,
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
                id: selected.id,
                url: selected.url,
                isPrimary: self.images.length == 0,
                thumbnail: selected.thumbnail.substr(
                  0,
                  selected.thumbnail.indexOf("?")
                ),
              });
            };
            self.mediaDialogData = res.model;
          }
        });
      });
    },
    methods: {
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
          images: self.images,
          description: self.description,
          attributes: self.attributes.map((m) => ({
            key: m.id,
            value: m.value,
          })),
          typeId: self.typeId,
          enable: self.enable,
          categories: self.categories,
          skus: JSON.parse(JSON.stringify(self.skus)),
          specifications: [],
        };

        model.skus.forEach((f) => {
          f.stock = f.stock - f.lastStock;
        });

        self.specifications.forEach((f) => {
          if (f.type == 0) {
            model.specifications.push(...f.options);
          }
        });

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
              key: specification.id,
              value: o.key,
              valueDisplay: o.value,
            });

            if (next) {
              recursion(next, coped);
            } else {
              coped.id = Kooboo.Guid.computeGuid(
                self.id + coped.specifications.map((m) => m.value).join("")
              );

              var old = self.skus.find((f) => f.id == coped.id);
              if (old) old.specifications = coped.specifications;
              skus.push(old || coped);
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
      initData(type, product) {
        var isNew = typeof product == "string";

        if (isNew) {
          product = null;
        } else {
          type = type[0];
          product = product[0];
          self.title = product.model.title;
          self.images = product.model.images;
          self.description = product.model.description;
          self.enable = product.model.enable;
        }

        var attributes = type.model.attributes;
        var specifications = type.model.specifications;
        var productAttributes = product ? product.model.attributes : [];
        var productSpecifications = product ? product.model.specifications : [];
        var skus = product ? product.model.skus : [];
        var skuSpecifications = [];

        for (const sku of skus) {
          sku.lastStock = sku.stock;
          skuSpecifications.push(...sku.specifications);
        }

        for (const attribute of attributes) {
          var attributeValue = productAttributes.find(
            (a) => a.key == attribute.id
          );
          attribute.value = attributeValue ? attributeValue.value : "";

          if (isNew && attribute.type == 1 && attribute.options.length) {
            attribute.value = attribute.options[0].id;
          }
        }

        for (const specification of specifications) {
          if (specification.type == 0) {
            specification.options = skuSpecifications
              .filter((f) => f.key == specification.id)
              .map((m) => ({
                key: m.value,
                value: productSpecifications.find((f) => f.key == m.value)
                  .value,
              }));
          }

          if (specification.type == 1) {
            specification.options.forEach(function (o) {
              o.checked = skuSpecifications.some((s) => s.value == o.key);
            });
          }
        }

        self.specifications = specifications;
        self.attributes = attributes;
        self.skus = skus;
        self.rebuildSkus();
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
