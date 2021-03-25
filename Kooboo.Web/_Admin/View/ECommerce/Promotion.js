$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        id: Kooboo.getQueryString("id"),
        defines: null,
        model: {
          id: Kooboo.Guid.NewGuid(),
          name: "",
          description: "",
          startTime: new Date(),
          endTime: new Date(),
          products: [],
          rules: null,
          exclusive: false,
          type: "MoneyOff",
          target: "Order",
          priority: 0,
          discount: 0,
        },
      };
    },
    mounted() {
      $.when(
        Kooboo.MatchRule.promotionDefines(),
        Kooboo.Product.keyValue(),
        this.id ? Kooboo.Promotion.get({ id: this.id }) : null
      ).then((define, keyValue, promotion) => {
        var defines = define[0].model;
        this.products = keyValue[0].model;
        var rules = {};

        if (promotion) {
          this.model = promotion[0].model;
        } else {
          for (const key in defines) {
            rules[key] = {
              type: "All",
              conditions: [],
            };
          }

          this.model.rules = rules;
        }

        this.defines = defines;
      });
    },
    methods: {
      saveAndReturn() {
        this.save();
        location.href = Kooboo.Route.Promotion.ListPage;
      },
      save() {
        Kooboo.Promotion.post(this.model);
      },
    },
  });
});
