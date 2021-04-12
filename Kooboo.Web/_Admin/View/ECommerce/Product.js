$(function () {
  new Vue({
    el: "#main",
    data: function () {
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        typeId: Kooboo.getQueryString("type"),
        id: Kooboo.getQueryString("id"),
        mediaDialogData: {},
        richeditor: {
          mediaDialogData: null,
          editorConfig: {
            min_height: 300,
            max_height: 600,
          },
        },
        productsUrl: Kooboo.Route.Product.ListPage,
        categories: [],
        type: null,
        oldSkus: [],
        model: {
          id: Kooboo.getQueryString("id"),
          title: "",
          images: [],
          description: "",
          attributes: [],
          specifications: [],
          typeId: Kooboo.getQueryString("type"),
          enable: true,
          skus: [],
        },
      };
    },
    mounted() {
      Kooboo.ProductType.Get({ id: this.model.typeId }).then((rsp) => {
        this.type = rsp.model;
        if (this.model.id) {
          Kooboo.Product.Get({ id: this.model.id }).then((rsp) => {
            this.model = rsp.model;
            this.initAttributes();
            this.oldSkus = JSON.parse(JSON.stringify(this.model.skus));
          });
        } else {
          this.model.id = Kooboo.Guid.NewGuid();
          this.initAttributes();
          this.initSpecifications();
        }
      });

      Kooboo.EventBus.subscribe("ko/style/list/pickimage/show", (ctx) => {
        Kooboo.Media.getList().then((res) => {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = ctx;
            res.model["onAdd"] = (selected) => {
              this.model.images.push({
                id: selected.id,
                url: selected.url,
                isPrimary: this.model.images.length == 0,
                thumbnail: selected.thumbnail.substr(
                  0,
                  selected.thumbnail.indexOf("?")
                ),
              });
            };
            this.mediaDialogData = res.model;
          }
        });
      });
    },
    methods: {
      initAttributes() {
        for (const i of this.type.attributes) {
          var exist = this.model.attributes.find((f) => f.key == i.id);
          if (!exist) {
            this.model.attributes.push({
              key: i.id,
              value: i.type == "Option" ? i.options[0].key : "",
            });
          }
        }
      },
      getAttribute(id) {
        return this.model.attributes.find((f) => f.key == id);
      },
      initSpecifications() {
        for (const i of this.type.specifications) {
          this.model.specifications.push({
            id: i.id,
            options: [],
            value: [],
            type: i.type,
          });
        }
      },
      getSpecification(id) {
        return this.model.specifications.find((f) => f.id == id);
      },
      setSpecification(id) {
        var s = this.getSpecification(id);
        s.value = s.options.map((m) => m.key);
      },
      switchOption(id, optionId) {
        var s = this.getSpecification(id);

        if (s.value.find((f) => f == optionId)) {
          s.value = s.value.filter((f) => f != optionId);
        } else {
          s.value.push(optionId);
        }
      },
      isCheckOption(id, optionId) {
        var s = this.getSpecification(id);
        return s.value.find((f) => f == optionId);
      },
      showMediaDialog: function () {
        Kooboo.EventBus.publish("ko/style/list/pickimage/show");
      },
      save: function (callback) {
        var coped = JSON.parse(JSON.stringify(this.model));
        var stocks = [];
        this.removeUnuseSkus(coped);
        var skus = coped.skus;
        delete coped.skus;

        for (const i of skus) {
          var sku = this.oldSkus.find((f) => f.id == i.id);
          var stock = sku ? i.stock - sku.stock : i.stock;
          stocks.push({ key: i.id, value: stock });
        }

        Kooboo.Product.post({
          product: coped,
          skus: skus,
          stocks: stocks,
        }).then(function (res) {
          if (res.success) {
            window.info.show(Kooboo.text.info.save.success, true);
            if (callback) callback();
          }
        });
      },
      saveAndReturn: function () {
        this.save(() => {
          location.href = this.productsUrl;
        });
      },
      imgUrlWrap(url) {
        return "url('" + url + this.querySiteId + "')";
      },
      setIsPrimary(item) {
        this.model.images.forEach((f) => (f.isPrimary = false));
        item.isPrimary = true;
      },
      removeImage(item) {
        this.model.images = this.model.images.filter((f) => f != item);
        if (this.model.images.length && item.isPrimary) {
          this.model.images[0].isPrimary = true;
        }
      },
      getSpecificationValue(specification, sku) {
        var kv = sku.find((f) => f.key == specification.id);

        var typeSpecification = this.type.specifications.find(
          (f) => f.id == specification.id
        );

        var options =
          specification.type == "Option"
            ? typeSpecification.options
            : specification.options;

        return options.find((f) => f.key == kv.value).value;
      },
      getSpecificationName(id) {
        return this.type.specifications.find((f) => f.id == id).name;
      },
      getOrNewSpecification(sku) {
        var s = this.model.skus.find(
          (f) => this.skuToString(f.specifications) == this.skuToString(sku)
        );

        if (!s) {
          s = {
            id: Kooboo.Guid.NewGuid(),
            name: "",
            price: 0,
            productId: this.model.id,
            specifications: sku,
            stock: 0,
            tax: 0,
            thumbnail: "",
            enable: true,
          };

          this.model.skus.push(s);
        }

        return s;
      },
      skuToString(sku) {
        return sku
          .map((m) => `${m.key}${m.value}`)
          .sort()
          .join("");
      },
      removeUnuseSkus(model) {
        var skus = this.skus.map((m) => this.skuToString(m));
        model.skus = model.skus.filter((f) => {
          var skuString = this.skuToString(f.specifications);
          return skus.find((f) => f == skuString) !== undefined;
        });
      },
    },
    computed: {
      skus() {
        var result = [];

        var recursion = (i, specifications) => {
          if (i > this.model.specifications.length - 1) return;
          var specification = this.model.specifications[i];

          for (var k = 0; k < specification.value.length; k++) {
            var list = JSON.parse(JSON.stringify(specifications));
            var value = specification.value[k];

            list.push({
              key: specification.id,
              value: value,
            });

            if (i == this.model.specifications.length - 1) {
              result.push(list);
            } else {
              recursion(i + 1, list);
            }
          }
        };

        recursion(0, []);

        if (this.model.specifications.length == 0) {
          result.push([]);
        }

        return result;
      },
    },
  });
});
