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
        selectedCategory: null,
        showCategoryModal: false,
        pager: {
          pageNr: 1,
          totalPages: 200,
        },
        list: [],
      };
    },
    mounted: function () {
      Kooboo.Product.getList({ index: 1, size: 5 }).then(function (res) {
        if (res.success) {
          self.list = res.model.list;
        }
      });
    },
    methods: {
      addProduct: function () {
        this.selectedCategory = null;
        this.showCategoryModal = true;
      },
      startAddProduct: function (id) {
        location.href = Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          category: id,
        });
      },
      editProduct: function (product) {
        return Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
          category: product.categoryId,
          id: product.id,
        });
      },
      getPrimaryImg: function (value) {
        var list = JSON.parse(value);
        if (!list.length) return;
        var item =
          list.filter(function (f) {
            return f.isPrimary;
          })[0] || list[0];

        return "url('" + item.url + self.querySiteId + "')";
      },
      changePage: function () {},
    },
    computed: {},
  });
});
