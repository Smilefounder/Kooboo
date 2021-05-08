$(function () {
  new Vue({
    el: "#main",
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
        customerId: Kooboo.getQueryString("id"),
        showProductModal: false,
        cart: null,
      };
    },
    mounted() {
      this.getData();
    },
    methods: {
      settlement() {
        location.href = Kooboo.Route.Get(Kooboo.Route.Cart.Settlement, {
          id: this.customerId,
        });
      },
      productVariantSelected(row) {
        var existItem = this.cart.items.find((f) => f.productVariantId == row.id);
        Kooboo.Cart.post({
          customerId: this.customerId,
          selected: existItem ? existItem.selected : true,
          productVariantId: row.id,
          quantity: existItem ? existItem.quantity + 1 : 1,
        }).then((rsp) => {
          this.showProductModal = false;
          this.getData();
        });
      },
      getData() {
        Kooboo.Cart.Get({ id: this.customerId }).then((rsp) => {
          this.cart = rsp.model;
        });
      },
      changeItem(id) {
        var item = this.cart.items.find((f) => f.id == id);
        Kooboo.Cart.post({
          customerId: this.customerId,
          selected: item.selected,
          productVariantId: item.productVariantId,
          quantity: item.quantity,
        }).then((rsp) => {
          this.getData();
        });
      },
      removeItem(id) {
        Kooboo.Cart.Deletes([id]).then((rsp) => {
          this.getData();
        });
      },
      specificationsDisplay(specifications) {
        return specifications.map((m) => `[${m.key}:${m.value}]`).join(" ");
      },
    },
  });
});
