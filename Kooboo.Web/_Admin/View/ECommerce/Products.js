$(function () {
  new Vue({
    el: "#app",
    data() {
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
        showModal: false,
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
        list: [],
        types: [],
        selectedRows: [],
        pageSize: 20,
      };
    },
    mounted() {
      this.changePage(1);

      Kooboo.ProductType.keyValue().then((res) => {
        if (res.success) {
          this.types = res.model;
        }
      });
    },
    methods: {
      startAddProduct(id) {
        location.href = Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          type: id,
        });
      },
      editProduct(product) {
        return Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          type: product.typeId,
          id: product.id,
        });
      },
      getPrimaryImg(list) {
        if (!list.length) return;
        var item =
          list.filter(function (f) {
            return f.isPrimary;
          })[0] || list[0];

        return "url('" + item.url + this.querySiteId + "')";
      },
      changePage(index) {
        Kooboo.Product.getList({ index: index, size: this.pageSize }).then(
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
        // var confirmStr = this.haveRelations
        //   ? Kooboo.text.confirm.deleteItemsWithRef
        //   : Kooboo.text.confirm.deleteItems;

        var confirmStr = Kooboo.text.confirm.deleteItemsWithRef;

        if (!confirm(confirmStr)) return;

        Kooboo.Product.Delete(this.selectedRows.map((m) => m.id)).then(
          (res) => {
            if (res.success) {
              this.changePage(this.pager.pageNr);
            }
          }
        );
      },
    }
  });
});
