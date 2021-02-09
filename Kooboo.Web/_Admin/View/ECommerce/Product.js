$(function () {
  var SITE_ID_QUERY_STRING = "?SiteId=" + Kooboo.getQueryString("SiteId");
  var categoryId = Kooboo.getQueryString("category") || Kooboo.Guid.Empty;
  var initTimes = 0;
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        categoryId: Kooboo.getQueryString("category"),
        category: null,
      };
    },
    mounted: function () {
      Kooboo.ProductCategory.executeGet("get", {
        id: self.categoryId,
      }).then(function (rsp) {
        if (!rsp.success) return;
        rsp.model.attributes = JSON.parse(rsp.model.attributes);
        rsp.model.specifications = JSON.parse(rsp.model.specifications);
        self.category = rsp.model;
      });
    },
    methods: {
      rmoveItem: function (items, item) {
        var index = items.indexOf(item);
        items.splice(index, 1);
      },
      addOption: function (item) {
        if (!item.editingItem.trim()) return;

        item.options.push({
          id: Kooboo.Guid.NewGuid(),
          value: item.editingItem,
        });

        item.editingItem = "";
      },
    },
    computed: {},
  });
});
