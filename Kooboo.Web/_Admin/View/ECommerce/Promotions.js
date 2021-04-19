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
            name: Kooboo.text.commerce.promotions,
          },
        ],
        list: null,
        selectedRows: [],
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
        pageSize: 20,
      };
    },
    mounted() {
      this.changePage(1);
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
          this.changePage(1);
        });
      },
      changePage(index) {
        Kooboo.Promotion.getList({ index: index, size: this.pageSize }).then(
          (res) => {
            if (res.success) {
              this.list = res.model.list;
              this.pager = {
                pageNr: res.model.pageIndex,
                totalPages: res.model.pageCount,
              };
            }
          }
        );
      },
      deletes() {
        var confirmStr = Kooboo.text.confirm.deleteItems;
        if (!confirm(confirmStr)) return;

        Kooboo.Promotion.Deletes(this.selectedRows.map((m) => m.id)).then(
          (rsp) => {
            if (!rsp.success) return;
            this.changePage(this.pager.pageNr);
          }
        );
      },
      getTargetDisplay(text) {
        var display = Kooboo.text.commerce.promotionTarget[text.toLowerCase()];
        return display ? display : text;
      },
    },
  });
});
