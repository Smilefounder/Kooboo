(function () {
  Kooboo.loadJS(["/_Admin/Scripts/kooboo/Guid.js"]);

  var countries = Kooboo.getTemplate(
    "/_Admin/Scripts/components/ECommerce/countries.json"
  );

  Vue.component("kb-consignee-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-consignee-modal.html"
    ),
    props: ["value", "id", "customerId"],
    data() {
      return {
        editData: null,
        countries: [],
        validateModel: {
          name: { valid: true, msg: "" },
          phone: { valid: true, msg: "" },
          state: { valid: true, msg: "" },
          city: { valid: true, msg: "" },
          street: { valid: true, msg: "" },
        },
        validRules: [
          { required: Kooboo.text.validation.required },
          {
            min: 2,
            max: 64,
            message:
              Kooboo.text.validation.minLength +
              2 +
              ", " +
              Kooboo.text.validation.maxLength +
              64,
          },
        ],
      };
    },
    computed: {
      show: {
        get: function () {
          return this.value;
        },
        set: function (value) {
          this.$emit("input", value);
        },
      },
      country() {
        if (!this.editData) return [];
        return this.countries.find((f) => f.code == this.editData.county);
      },
    },
    mounted() {
      this.countries = JSON.parse(countries);
    },
    methods: {
      save() {
        if (!this.valid()) return;

        Kooboo.Consignee.post(this.editData).then((rsp) => {
          this.$emit("ok");
          this.show = false;
        });
      },
      getDefaultCounty() {
        try {
          return (navigator.language || navigator.userLanguage).split("-")[1];
        } catch (error) {
          return "";
        }
      },
      valid() {
        var valid = true;

        this.validateModel.name = Kooboo.validField(
          this.editData.name,
          this.validRules
        );

        if (!this.validateModel.name.valid) valid = false;

        this.validateModel.phone = Kooboo.validField(
          this.editData.phone,
          this.validRules
        );

        if (!this.validateModel.phone.valid) valid = false;

        this.validateModel.state = Kooboo.validField(
          this.editData.state,
          this.validRules
        );

        if (!this.validateModel.state.valid) valid = false;

        this.validateModel.city = Kooboo.validField(
          this.editData.city,
          this.validRules
        );

        if (!this.validateModel.city.valid) valid = false;

        this.validateModel.street = Kooboo.validField(
          this.editData.street,
          this.validRules
        );

        if (!this.validateModel.street.valid) valid = false;

        return valid;
      },
    },
    watch: {
      value(value) {
        if (!value) return;
        if (this.id) {
          Kooboo.Consignee.get({ id: this.id }).then((rsp) => {
            this.editData = rsp.model;
          });
        } else {
          this.editData = {
            id: Kooboo.Guid.NewGuid(),
            customerId: this.customerId,
            county: this.getDefaultCounty(),
            state: "",
            city: "",
            street: "",
            address: "",
            name: "",
            phone: "",
          };
        }
      },
      editData: {
        handler() {
          for (const key in this.validateModel) {
            this.validateModel[key] = { valid: true, msg: "" };
          }
        },
        deep: true,
      },
    },
  });
})();
