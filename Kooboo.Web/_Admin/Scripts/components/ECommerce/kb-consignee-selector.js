(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/ECommerce/kb-consignee-modal.js",
  ]);

  Vue.component("kb-consignee-selector", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-consignee-selector.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        list: [],
        showModal: false,
        editData: null,
      };
    },
    mounted() {
      this.getList();
    },
    methods: {
      add() {
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
        this.showModal = true;
      },
      getList() {
        Kooboo.Consignee.getList({
          id: this.id,
        }).then((rsp) => {
          this.list = rsp.model;
          if (!this.value && this.list.length) {
            this.$emit("input", this.list[0].id);
          }
        });
      },
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
  });
})();
