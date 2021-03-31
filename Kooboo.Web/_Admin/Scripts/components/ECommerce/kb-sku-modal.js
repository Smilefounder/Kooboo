(function () {
  Vue.component("kb-sku-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-sku-modal.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        selectedRows: [],
        list: [],
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
        this.$emit("ok", this.selectedRows);
        this.show = false;
      },
    },
    watch: {
      show(value) {
        if (value) {
          Kooboo.Product.skuList({ id: this.id }).then((rsp) => {
            this.list = rsp.model;
          });
        } else {
          this.list = [];
        }
      },
    },
  });
})();
