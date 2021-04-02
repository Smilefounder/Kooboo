$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        order: null,
        model: {
          customerId: Kooboo.getQueryString("id"),
          consigneeId: "",
          paymentMethod: "",
        },
      };
    },
    mounted() {
      Kooboo.Order.preview({
        id: this.model.customerId,
      }).then((rsp) => {
        this.order = rsp.model;
      });
    },
    methods: {
      specificationsDisplay(specifications) {
        return specifications.map((m) => `[${m.key}:${m.value}]`).join(" ");
      },
    },
  });
});
