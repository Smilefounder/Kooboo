$(function () {
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
          startTime: "",
          endTime: "",
          products: [],
          rules: null,
          exclusive: false,
          type: "MoneyOff",
          target: "Order",
          priority: 0,
          discount: 0,
          enable: true,
        },
        validateModel: {
          name: { valid: true, msg: "" },
          startTime: { valid: true, msg: "" },
          endTime: { valid: true, msg: "" },
          discount: { valid: true, msg: "" },
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
          this.model.startTime = moment(this.model.startTime).format(
            "YYYY-MM-DD HH:mm"
          );
          this.model.endTime = moment(this.model.endTime).format(
            "YYYY-MM-DD HH:mm"
          );
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
        this.save(() => {
          location.href = Kooboo.Route.Promotion.ListPage;
        });
      },
      save(callback) {
        if (!this.valid()) return;

        Kooboo.Promotion.post(this.model).then((rsp) => {
          if (rsp.success && callback instanceof Function) {
            callback();
          }
        });
      },
      getTargetDisplay(text) {
        var display = Kooboo.text.commerce.promotionTarget[text.toLowerCase()];
        return display ? display : text;
      },
      valid() {
        var valid = true;

        this.validateModel.name = Kooboo.validField(this.model.name, [
          { required: Kooboo.text.validation.required },
          {
            min: 2,
            max: 30,
            message:
              Kooboo.text.validation.minLength +
              2 +
              ", " +
              Kooboo.text.validation.maxLength +
              30,
          },
        ]);

        if (!this.validateModel.name.valid) valid = false;

        this.validateModel.startTime = Kooboo.validField(this.model.startTime, [
          { required: Kooboo.text.validation.required },
        ]);

        if (!this.validateModel.startTime.valid) valid = false;

        this.validateModel.endTime = Kooboo.validField(this.model.endTime, [
          { required: Kooboo.text.validation.required },
        ]);

        if (!this.validateModel.endTime.valid) valid = false;

        if (this.model.type == "MoneyOff") {
          this.validateModel.discount = Kooboo.validField(this.model.discount, [
            { required: Kooboo.text.validation.required },
            {
              validate: (value) => value > 0,
              message: Kooboo.text.validation.greaterThan + "0",
            },
          ]);
        } else {
          this.validateModel.discount = Kooboo.validField(this.model.discount, [
            { required: Kooboo.text.validation.required },
            {
              validate: (value) => value > 0 && value < 100,
              message:
                Kooboo.text.validation.greaterThan +
                "0" +
                Kooboo.text.validation.lessThan +
                "100",
            },
          ]);
        }

        if (!this.validateModel.discount.valid) valid = false;

        return valid;
      },
    },
    watch: {
      model: {
        handler() {
          for (const key in this.validateModel) {
            this.validateModel[key] = { valid: true, msg: "" };
          }
        },
        deep: true,
      },
    },
  });
});
