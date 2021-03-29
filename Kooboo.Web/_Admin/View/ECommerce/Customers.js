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
        selectedRows: [],
        showRegisterModal: false,
        registerModel: null,
        pageSize: 20,
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
      };
    },
    mounted() {
      this.changePage(1);
    },
    methods: {
      cartPage(id) {
        return Kooboo.Route.Get(Kooboo.Route.Customer.Cart, {
          id: id,
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
        Kooboo.Customer.getList({ index: 1, size: 5 }).then((res) => {
          if (res.success) {
            this.list = res.model.list;
            this.pager = {
              pageNr: res.model.pageIndex,
              totalPages: res.model.pageCount,
            };
          }
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
      changePage(index) {
        Kooboo.Customer.getList({ index: index, size: this.pageSize }).then(
          (res) => {
            if (res.success) {
              this.list = res.model.list;
              this.pager = {
                pageNr: res.model.pageIndex,
                totalPages: res.model.pageCount,
              };
            }
          }
        );
      },
      saveRegister() {
        Kooboo.Customer.register(this.registerModel).then((rsp) => {
          this.changePage(1);
          this.showRegisterModal = false;
        });
      },
      registerCustomer() {
        this.registerModel = { userName: "", password: "" };
        this.showRegisterModal = true;
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
