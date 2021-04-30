(function () {
  Kooboo.loadJS(["/_Admin/Scripts/kooboo/Guid.js"]);

  Vue.component("kb-consignee-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-consignee-modal.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        editData: null,
        countries: [],
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
    },
    mounted() {
      Kooboo.Region.countries().then((rsp) => {
        this.countries = rsp.model;
      });
    },
    methods: {
      edit(id) {
        Kooboo.Consignee.Get({ id: id }).then((rsp) => {
          this.editData = rsp.model;
          this.showModal = true;
        });
      },
      Delete(id) {
        Kooboo.Consignee.Delete({ id: id }).then((rsp) => {
          this.getList();
        });
      },
      save() {
        Kooboo.Consignee.post(this.editData).then((rsp) => {
          this.getList();
          this.showModal = false;
        });
      },
    },
    watch: {
      value(value) {
        if (!value) return;
        if (this.id) {
          //
        } else {
          this.editData = {
            id: Kooboo.Guid.NewGuid(),
            customerId: this.id,
            county: "",
            state: "",
            city: "",
            address: "",
            name: "",
            phone: "",
            email: "",
          };
        }
      },
    },
  });
})();
