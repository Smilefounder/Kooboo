$(function () {
  new Vue({
    el: "#app",
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
            name: "Promotions",
          },
        ],
        list: null,
        selectedRows: [],
      };
    },
    mounted() {
      this.getData();
    },
    methods: {
      add() {
        location.href = Kooboo.Route.Get(Kooboo.Route.Promotion.DetailPage);
      },
      edit(id) {
        return Kooboo.Route.Get(Kooboo.Route.Promotion.DetailPage, {
          id: id,
        });
      },
      save() {
        Kooboo.Category.post(this.editData).then((rsp) => {
          if (!rsp.success) return;
          this.getData();
        });
      },
      getData() {
        Kooboo.Promotion.getList().then((rsp) => {
          this.list = rsp.model;
        });
      },
      deletes() {
        Kooboo.Promotion.Delete(this.selectedRows.map((m) => m.id)).then(
          (rsp) => {
            if (!rsp.success) return;
            this.getData();
          }
        );
      },
    },
  });
});
