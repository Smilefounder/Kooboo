$(function () {
  var self;
  new Vue({
    el: "#app",
    data: function () {
      self = this;
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        breads: [
          {
            name: "SITES",
          },
          {
            name: "DASHBOARD",
          },
          {
            name: Kooboo.text.common.ProductTypes,
          },
        ],
        selectedRows: [],
        showModal: false,
        list: [],
        id: null,
        productList: [],
        showProductModal: false,
      };
    },
    mounted: function () {
      this.getList();
    },
    computed: {
      haveRelations() {
        return this.selectedRows.some((s) => s.productCount);
      },
    },
    methods: {
      add() {
        this.id = null;
        this.showModal = true;
      },
      deletes() {
        var confirmStr = this.haveRelations
          ? Kooboo.text.confirm.deleteItemsWithRef
          : Kooboo.text.confirm.deleteItems;

        if (!confirm(confirmStr)) return;

        Kooboo.ProductType.Delete(this.selectedRows.map((m) => m.id)).then(
          (res) => {
            if (res.success) {
              this.getList();
            }
          }
        );
      },
      getList() {
        Kooboo.ProductType.getList().then((res) => {
          if (res.success) this.list = res.model;
        });
      },
      edit(id) {
        this.id = id;
        this.showModal = true;
      },
      showProducts(row) {
        Kooboo.Product.keyValue({ typeId: row.id }).then((res) => {
          if (res.success) {
            this.productList = res.model;
            this.showProductModal = true;
          }
        });
      },
    },
  });
});
