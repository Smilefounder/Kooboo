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
        list: null,
        selectedRows: [],
        showProductModal: false,
        selectorProducts: [],
        cart: null,
      };
    },
    mounted() {
      this.getData();
    },
    methods: {
      saveAndReturn() {},
      save() {},
      order() {},
      productSelected() {},
      getData() {
        $.when(
          Kooboo.Product.keyValue(),
          Kooboo.Cart.Get({ id: Kooboo.getQueryString("id") })
        ).then((products, cart) => {
          this.selectorProducts = products[0].model;
          this.cart = cart[0].model;
        });
      },
    },
    computed: {},
  });
});
