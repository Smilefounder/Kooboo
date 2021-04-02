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
        showSkuModal: false,
        products: [],
        addProductId: null,
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
      productSelected(rows) {
        if (rows.length == 1) {
          this.addProductId = rows[0].key;
          this.showSkuModal = true;
        }
      },
      skuSelected(row) {
        if (row.length == 1) {
          var existItem = this.cart.items.find((f) => f.skuId == row[0].id);
          Kooboo.Cart.post({
            customerId: this.customerId,
            selected: existItem ? existItem.selected : true,
            productId: row[0].productId,
            skuId: row[0].id,
            quantity: existItem ? existItem.quantity + 1 : 1,
          }).then((rsp) => {
            this.showSkuModal = false;
            this.getData();
          });
        }
      },
      getData() {
        $.when(
          Kooboo.Product.keyValue(),
          Kooboo.Cart.Get({ id: this.customerId })
        ).then((products, cart) => {
          this.products = products[0].model;
          this.cart = cart[0].model;
        });
      },
      changeItem(id) {
        var item = this.cart.items.find((f) => f.id == id);
        Kooboo.Cart.post({
          customerId: this.customerId,
          selected: item.selected,
          productId: item.productId,
          skuId: item.skuId,
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
