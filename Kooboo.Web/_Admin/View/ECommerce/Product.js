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
        mediaDialogData: {},
        title: "",
        images: [],
        description: "",
        attributes: [],
        richeditor: {
          mediaDialogData: null,
          editorConfig: {
            min_height: 300,
            max_height: 600,
          },
        },
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

      Kooboo.EventBus.subscribe("ko/style/list/pickimage/show", function (ctx) {
        Kooboo.Media.getList().then(function (res) {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = ctx;
            res.model["onAdd"] = function (selected) {
              self.images.push(selected.url);
            };
            self.mediaDialogData = res.model;
          }
        });
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
      showMediaDialog: function () {
        Kooboo.EventBus.publish("ko/style/list/pickimage/show");
      },
    },
    computed: {
      imagesDisplay: function () {
        return this.images.map(function (m) {
          return "url('" + m + SITE_ID_QUERY_STRING + "')";
        });
      },
    },
  });
});
