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
        editDataRules: {
          name: [{ required: Kooboo.text.validation.required }],
        },
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
        if (!this.$refs.form.validate()) return;

        Kooboo.ProductType.post(this.editData).then((res) => {
          if (res.success) {
            this.show = false;
            this.$emit("ok", this.editData.id);
          }
        });
      },
      addItem(items) {
        items.push({
          id: Kooboo.Guid.NewGuid(),
          name: "",
          type: 0,
          editingItem: "",
          options: [],
        });
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
    },
  });
})();
