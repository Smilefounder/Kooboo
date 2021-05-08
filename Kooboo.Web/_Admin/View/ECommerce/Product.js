$(function () {
  new Vue({
    el: "#main",
    data() {
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
        oldProductVariants: [],
        showSelectorModal: false,
        model: {
          id: Kooboo.getQueryString("id"),
          title: "",
          images: [],
          description: "",
          attributes: [],
          specifications: [],
          typeId: Kooboo.getQueryString("type"),
          enable: true,
          productVariants: [],
          categories: [],
        },
        validateModel: {
          title: { valid: true, msg: "" },
        },
        validRules: [
          { required: Kooboo.text.validation.required },
          {
            min: 0,
            message: Kooboo.text.validation.minLength + 0,
          },
        ],
      };
    },
    mounted() {
      Kooboo.Category.keyValue().then((rsp) => {
        this.categories = rsp.model;
      });

      Kooboo.ProductType.Get({ id: this.model.typeId }).then((rsp) => {
        this.type = rsp.model;

        for (const i of this.type.specifications) {
          Vue.set(this.validateModel, i.id, { valid: true, msg: "" });
        }

        if (this.model.id) {
          Kooboo.Product.Get({ id: this.model.id }).then((rsp) => {
            this.model = rsp.model;
            this.initAttributes();

            for (const i of this.model.productVariants) {
              this.initValidateModelItem(i.id);
            }

            this.oldProductVariants = JSON.parse(JSON.stringify(this.model.productVariants));
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
              var image = {
                id: selected.id,
                url: selected.url,
                isPrimary: ctx ? true : this.model.images.length == 0,
                thumbnail: selected.thumbnail.substr(
                  0,
                  selected.thumbnail.indexOf("?")
                ),
              };

              if (ctx) {
                Vue.set(ctx, "image", image);
              } else {
                this.model.images.push(image);
              }
            };
            this.mediaDialogData = res.model;
          }
        });
      });
    },
    methods: {
      getCategoryName(id) {
        var category = this.categories.find((f) => f.key == id);
        return category ? category.value : id;
      },
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
        this.removeUnuseProductVariants(coped);
        if (!this.valid(coped)) return;
        var productVariants = coped.productVariants;
        var categories = coped.categories;
        delete coped.productVariants;
        delete coped.categories;

        for (const i of productVariants) {
          var productVariant = this.oldProductVariants.find((f) => f.id == i.id);
          var stock = productVariant ? i.stock - productVariant.stock : i.stock;
          stocks.push({ key: i.id, value: stock });
        }

        Kooboo.Product.post({
          product: coped,
          productVariants: productVariants,
          stocks: stocks,
          categories,
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
      getSpecificationValue(specification, productVariant) {
        var kv = productVariant.find((f) => f.key == specification.id);

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
      getOrNewSpecification(productVariant) {
        var s = this.model.productVariants.find(
          (f) => this.productVariantToString(f.specifications) == this.productVariantToString(productVariant)
        );

        if (!s) {
          s = {
            id: Kooboo.Guid.NewGuid(),
            name: "",
            price: 0,
            productId: this.model.id,
            specifications: productVariant,
            stock: 0,
            tax: 0,
            thumbnail: "",
            enable: true,
            image: null,
          };

          this.initValidateModelItem(s.id);
          this.model.productVariants.push(s);
        }

        return s;
      },
      productVariantToString(productVariant) {
        return productVariant
          .map((m) => `${m.key}${m.value}`)
          .sort()
          .join("");
      },
      removeUnuseProductVariants(model) {
        var productVariants = this.productVariants.map((m) => this.productVariantToString(m));
        model.productVariants = model.productVariants.filter((f) => {
          var productVariantString = this.productVariantToString(f.specifications);
          return productVariants.find((f) => f == productVariantString) !== undefined;
        });
      },
      valid(model) {
        var valid = true;

        this.validateModel.title = Kooboo.validField(model.title, [
          { required: Kooboo.text.validation.required },
          {
            min: 2,
            max: 256,
            message:
              Kooboo.text.validation.minLength +
              2 +
              ", " +
              Kooboo.text.validation.maxLength +
              256,
          },
        ]);

        if (!this.validateModel.title.valid) valid = false;

        var validGroup = (item, prop) => {
          this.validateModel[item.id + prop] = Kooboo.validField(
            item[prop],
            this.validRules
          );
          if (!this.validateModel[item.id + prop].valid) valid = false;
        };

        var optionEditors = this.$refs.optionsEditors;

        if (optionEditors) {
          for (const i of optionEditors) {
            if (!i.valid()) valid = false;
          }
        }

        for (const i of model.specifications) {
          this.validateModel[i.id] = Kooboo.validField(
            i.value.length ? true : undefined,
            [{ required: Kooboo.text.validation.required }]
          );

          if (!this.validateModel[i.id].valid) valid = false;
        }

        for (const i of model.productVariants) {
          validGroup(i, "price");
          validGroup(i, "tax");
          validGroup(i, "stock");
        }

        return valid;
      },
      initValidateModelItem(id) {
        Vue.set(this.validateModel, id + "price", { valid: true, msg: "" });
        Vue.set(this.validateModel, id + "tax", { valid: true, msg: "" });
        Vue.set(this.validateModel, id + "stock", { valid: true, msg: "" });
      },
      selectProductVariantImage(productVariant) {
        Kooboo.EventBus.publish("ko/style/list/pickimage/show", productVariant);
      },
      categoriesSelected(selected) {
        this.model.categories = selected.map((m) => m.key);
      },
    },
    computed: {
      productVariants() {
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
      selecteds() {
        return this.categories.filter((f) =>
          this.model.categories.includes(f.key)
        );
      },
    },
    watch: {
      model: {
        handler() {
          for (const key in this.validateModel) {
            this.validateModel[key] = { valid: true, msg: "" };
          }
        },
        deep: true,
      },
    },
  });
});
