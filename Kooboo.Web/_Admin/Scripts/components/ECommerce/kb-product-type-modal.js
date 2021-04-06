(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/kbForm.js",
    "/_Admin/Scripts/components/ECommerce/kb-options-editor.js",
  ]);

  Vue.component("kb-product-type-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-product-type-modal.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        editData: null,
        isEdit: false,
        validateModel: {
          name: { valid: true, msg: "" },
        },
        validRules: [
          { required: Kooboo.text.validation.required },
          {
            min: 2,
            max: 31,
            message:
              Kooboo.text.validation.minLength +
              2 +
              ", " +
              Kooboo.text.validation.maxLength +
              31,
          },
        ],
      };
    },
    computed: {
      show: {
        get: function () {
          return this.value;
        },
        set: function (value) {
          this.$emit("input", value);
        },
      },
    },
    methods: {
      save() {
        if (!this.valid()) return;

        Kooboo.ProductType.post(this.editData).then((res) => {
          if (res.success) {
            this.show = false;
            this.$emit("ok", this.editData.id);
          }
        });
      },
      addItem(items) {
        var id = Kooboo.Guid.NewGuid();
        Vue.set(this.validateModel, id, { valid: true, msg: "" });

        items.push({
          id: id,
          name: "",
          type: 0,
          editingItem: "",
          options: [],
        });
      },
      valid() {
        var valid = true;

        this.validateModel.name = Kooboo.validField(
          this.editData.name,
          this.validRules
        );

        if (!this.validateModel.name.valid) valid = false;

        var validGroup = (items) => {
          for (const i of items) {
            this.validateModel[i.id] = Kooboo.validField(
              i.name,
              this.validRules
            );
            if (!this.validateModel[i.id].valid) valid = false;
          }
        };

        validGroup(this.editData.attributes);
        validGroup(this.editData.specifications);

        var optionEditors = this.$refs.optionsEditors;
        if (optionEditors) {
          for (const i of optionEditors) {
            if (!i.valid()) valid = false;
          }
        }

        return valid;
      },
    },
    watch: {
      value(value) {
        if (!value) return;
        this.isEdit = !!this.id;

        if (this.id) {
          Kooboo.ProductType.get({ id: this.id }).then((res) => {
            if (res.success) {
              this.editData = res.model;

              for (const i of this.editData.attributes) {
                Vue.set(this.validateModel, i.id, { valid: true, msg: "" });
              }

              for (const i of this.editData.specifications) {
                Vue.set(this.validateModel, i.id, { valid: true, msg: "" });
              }

              this.show = true;
            }
          });
        } else {
          this.editData = {
            id: Kooboo.Guid.NewGuid(),
            name: "",
            attributes: [],
            specifications: [],
          };
          this.show = true;
        }
      },
      editData: {
        handler() {
          for (const key in this.validateModel) {
            this.validateModel[key] = { valid: true, msg: "" };
          }
        },
        deep: true,
      },
    },
  });
})();
