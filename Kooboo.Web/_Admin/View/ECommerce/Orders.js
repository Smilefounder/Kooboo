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
            name: Kooboo.text.common.ProductCategories,
          },
        ],
        list: null,
        selectedRows: [],
        pageSize: 20,
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
      };
    },
    mounted() {
      this.changePage(1);
    },
    methods: {
      changePage(index) {
        Kooboo.Order.getList({ index: index, size: this.pageSize }).then(
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
      orderDetail(id) {
        return Kooboo.Route.Get(Kooboo.Route.Order.DetailPage, {
          id: id,
        });
      },
    },
  });
});
