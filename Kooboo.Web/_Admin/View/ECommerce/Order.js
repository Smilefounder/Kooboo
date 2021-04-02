$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        id: Kooboo.getQueryString("id"),
        order: null,
      };
    },
    mounted() {
      Kooboo.Order.get({
        id: this.id,
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
