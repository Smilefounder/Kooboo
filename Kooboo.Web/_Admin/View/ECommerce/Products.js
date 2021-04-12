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
      };
    },
    mounted: function () {
      Kooboo.Product.getList({ index: 1, size: 5 }).then(function (res) {
        if (res.success) {
          self.list = res.model.list;
        }
      });

      Kooboo.ProductType.keyValue().then(function (res) {
        if (res.success) {
          self.types = res.model;
        }
      });
    },
    methods: {
      startAddProduct: function (id) {
        location.href = Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          type: id,
        });
      },
      editProduct: function (product) {
        return Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          type: product.typeId,
          id: product.id,
        });
      },
      getPrimaryImg: function (list) {
        if (!list.length) return;
        var item =
          list.filter(function (f) {
            return f.isPrimary;
          })[0] || list[0];

        return "url('" + item.url + self.querySiteId + "')";
      },
      changePage() {},
    },
    computed: {},
  });
});
