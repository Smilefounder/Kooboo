$(function () {
  var CONTENT_ID = Kooboo.getQueryString("id");
  var SITE_ID_QUERY_STRING = "?SiteId=" + Kooboo.getQueryString("SiteId");
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
        logistics: [],
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
          logisticsCompany: [
            {
              required: true,
              message: Kooboo.text.validation.required,
            },
          ],
          logisticsNumber: [
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
      this.getAllLogistics();
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
              if (item.variants && item.variants.thumbnail) {
                item.variants.thumbnail =
                  "/_thumbnail/80/80" +
                  item.variants.thumbnail +
                  SITE_ID_QUERY_STRING;
              }
            });
            self.model = res.model;
          }
        });
      },
      getAllLogistics: function () {
        Kooboo.Order.getAllLogistics().then(function (res) {
          if (res.success) {
            self.logistics = res.model;
          }
        });
      },
      getSaveOrder: function () {
        return {
          id: self.model.id,
          customerAddress: self.model.customerAddress,
          logisticsCompany: self.model.logisticsCompany,
          logisticsNumber: self.model.logisticsNumber
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
    computed: {
      isShipped() {
        return ~["Shipping", "Finished"].indexOf(self.model.status);
      },
    },
  });
});
