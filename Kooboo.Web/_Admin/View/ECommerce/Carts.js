$(function () {
  var self;
  new Vue({
    el: "#app",
    data: function () {
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
            name: Kooboo.text.common.ProductManagement,
          },
        ],
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
        list: [],
        pageSize: 20,
      };
    },
    mounted() {
      this.changePage(1);
    },
    methods: {
      changePage(index) {
        Kooboo.Cart.getList({ index: index, size: this.pageSize }).then(
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
      specificationsDisplay(specifications) {
        return specifications.map((m) => `[${m.key}:${m.value}]`).join(" ");
      },
      cartPage(id) {
        return Kooboo.Route.Get(Kooboo.Route.Customer.Cart, {
          id: id,
        });
      },
    },
  });
});
