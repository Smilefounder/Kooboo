$(function () {
  new Vue({
    el: "#app",
    data: function () {
      return {
        breads: [
          {
            name: "SITES",
          },
          {
            name: "DASHBOARD",
          },
          {
            name: Kooboo.text.common.ProductCategories,
          },
        ],
        list: null,
        defines: null,
        selectedRows: [],
        showModal: false,
        editData: null,
        showSelectorModal: false,
        products: [],
      };
    },
    mounted() {
      this.getData();
    },
    methods: {
      add() {
        this.editData = {
          id: Kooboo.Guid.NewGuid(),
          name: "",
          type: "Manual",
          rule: {
            type: "",
            conditions: [],
          },
          products: [],
        };

        this.showModal = true;
      },
      
      edit(id) {
        Kooboo.Category.get({ id: id }).then((rsp) => {
          if (!rsp.success) return;
          this.editData = rsp.model;
          this.showModal = true;
        });
      },
      productSelected(rows) {
        this.editData.products.push(...rows.map((m) => m.key));
      },
      save() {
        Kooboo.Category.post(this.editData).then((rsp) => {
          if (!rsp.success) return;
          this.getData();
          this.showModal = false;
        });
      },
      getData() {
        $.when(
          Kooboo.Category.getList(),
          Kooboo.MatchRule.categoryDefines(),
          Kooboo.Product.keyValue()
        ).then((categories, define, keyValue) => {
          this.list = categories[0].model;
          this.defines = define[0].model;
          this.products = keyValue[0].model;
        });
      },
      deletes() {
        Kooboo.Category.Delete(this.selectedRows.map((m) => m.id)).then(
          (rsp) => {
            if (!rsp.success) return;
            this.getData();
          }
        );
      },
    },
    computed: {
      selectorProducts() {
        var selectedProducts = this.editData ? this.editData.products : [];
        var result = [];

        for (const product of this.products) {
          if (!selectedProducts.find((f) => f == product.key)) {
            result.push(product);
          }
        }

        return result;
      },
      selectedProducts() {
        var selectedProducts = this.editData ? this.editData.products : [];
        var result = [];

        for (const product of this.products) {
          if (selectedProducts.find((f) => f == product.key)) {
            result.push(product);
          }
        }

        return result;
      },
    },
  });
});
