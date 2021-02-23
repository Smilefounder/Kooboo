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
        };

        model.skus.forEach((f) => {
          f.stock = f.stock - f.lastStock;
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
          self.images = JSON.parse(product.model.images);
          self.description = product.model.description;
          self.enable = product.model.enable;
        }

        var attributes = type.model.attributes;
        var specifications = type.model.specifications;
        var productAttributes = product ? product.model.attributes : [];
debugger
        attributes.forEach((f) => {
          var attribute = productAttributes.find((a) => a.key == f.id);
          f.value = attribute ? attribute.value : "";

          if (isNew && f.type == 1 && f.options.length) {
            f.value = f.options[0].id;
          }
        });

        specifications.forEach((f) => {
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
