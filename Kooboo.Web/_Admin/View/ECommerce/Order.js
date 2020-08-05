$(function () {
  var CONTENT_ID = Kooboo.getQueryString("id");
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        id: CONTENT_ID || Kooboo.Guid.Empty,
        isNewContent: !CONTENT_ID,
        mediaDialogData: {},
        fields: [],
        siteLangs: null,
        contentValues: {},
        model: {},
        addressRules: {
          consignee: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
          contactNumber: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
          country: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
          city: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
          address: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
        },
      };
    },
    mounted: function () {
      Kooboo.Site.Langs().then(function (langRes) {
        if (langRes.success) {
          self.siteLangs = langRes.model;
        }
        self.getContentFields();
      });
      Kooboo.EventBus.subscribe("ko/style/list/pickimage/show", function (ctx) {
        Kooboo.Media.getList().then(function (res) {
          if (res.success) {
            res.model["show"] = true;
            res.model["context"] = ctx;
            res.model["onAdd"] = function (selected) {
              ctx.settings.file_browser_callback(
                ctx.field_name,
                selected.url + "?SiteId=" + Kooboo.getQueryString("SiteId"),
                ctx.type,
                ctx.win,
                true
              );
            };
            self.mediaDialogData = res.model;
          }
        });
      });
    },
    methods: {
      getContentFields: function () {
        var params = {};
        if (CONTENT_ID) {
          params.id = self.id;
        }
        Kooboo.Order.getEdit(params).then(function (res) {
          if (res.success) {
            res.model.createDate = new Date(
              res.model.createDate
            ).toDefaultLangString();
            res.model.items.forEach(function (item) {
              item.url = Kooboo.Route.Get(Kooboo.Route.Product.DetailPage, {
                id: item.product.id,
                type: item.product.productTypeId,
              });
            });
            self.model = res.model;
          }
        });
      },
      getSaveOrder: function () {
        return {
          ...self.model,
        };
      },
      isAbleToSaveOrder: function () {
        if (!this.$refs.addressForm) {
          return true;
        }
        return this.$refs.addressForm.validate();
      },
      onSubmit: function (cb) {
        if (self.isAbleToSaveOrder()) {
          Kooboo.Order.editOrder(self.getSaveOrder()).then(function (res) {
            if (res.success) {
              if (cb && typeof cb == "function") {
                cb(res.model);
              }
            }
          });
        }
      },
      onContentSave: function () {
        self.onSubmit(function (order) {
          location.href = Kooboo.Route.Get(Kooboo.Route.Order.DetailPage, {
            id: order.id,
          });
        });
      },
      onContentSaveAndCreate: function () {
        self.onSubmit(function () {
          window.info.done(Kooboo.text.info.save.success);
          setTimeout(function () {
            location.href = Kooboo.Route.Get(Kooboo.Route.Order.DetailPage);
          }, 300);
        });
      },
      onContentSaveAndReturn: function () {
        self.onSubmit(function () {
          location.href = Kooboo.Route.Get(Kooboo.Route.Order.ListPage);
        });
      },
      userCancel: function () {
        location.href = Kooboo.Route.Get(Kooboo.Route.Order.ListPage);
      },
    },
  });
});
