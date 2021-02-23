$(function () {
  var self;
  new Vue({
    el: "#app",
    data: function () {
      self = this;
      return {
        querySiteId: "?SiteId=" + Kooboo.getQueryString("SiteId"),
        breads: [
          {
            name: "SITES",
          },
          {
            name: "DASHBOARD",
          },
          {
            name: Kooboo.text.common.ProductTypes,
          },
        ],
        selectedRows: [],
        showModal: false,
        list: [],
        id: null,
      };
    },
    mounted: function () {
      this.getList();
    },
    methods: {
      add() {
        this.id = null;
        this.showModal = true;
      },
      deletes() {
        Kooboo.ProductType.Delete(this.selectedRows.map((m) => m.id)).then(
          (res) => {
            if (res.success) {
              this.getList();
            }
          }
        );
      },
      getList() {
        Kooboo.ProductType.getList().then((res) => {
          if (res.success) this.list = res.model;
        });
      },
      edit(id) {
        this.id = id;
        this.showModal = true;
      },
    },
  });
});
