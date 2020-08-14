$(function () {
  var self;
  new Vue({
    el: "#app",
    data: function () {
      self = this;
      return {
        breads: [
          {
            name: "SITES",
          },
          {
            name: "DASHBOARD",
          },
          {
            name: Kooboo.text.common.OrdersManagement,
          },
        ],
        tableData: [],
        selected: [],
        newOrder: Kooboo.Route.Get(Kooboo.Route.Order.DetailPage),
        pager: {},
        searchKey: "",
        cacheData: null,
        isSearching: false,
        visibleShipModal: false,
        currentOrder: {},
        logistics: [],
        shipModel: {
          logisticsCompany: "",
          logisticsNumber: "",
        },
        shipRules: {
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
        currentPageNr: 1,
        status: '',
        statuses: []
      };
    },
    created() {
      var statuses = ["Created", "Paid", "Shipping", "Finished", "Cancel"];
      this.statuses = statuses.map(function(item) {
        return {
          value: item,
          label: Kooboo.text.eCommerce.orderStatus[item]
        }
      });
    },
    mounted: function () {
      self.getList();
    },
    methods: {
      getList: function (page) {
        this.currentPageNr = page;
        Kooboo.Order.getList({
          pageNr: page,
        }).then(function (res) {
          if (res.success) {
            self.cacheData = res.model;
            self.handleData(res.model);
          }
        });
      },
      searchStart: function () {
        if (this.searchKey || this.status) {
          Kooboo.Order.search({
            keyword: self.searchKey,
            status: self.status,
          }).then(function (res) {
            if (res.success) {
              self.handleData(res.model);
              self.isSearching = true;
            }
          });
        } else {
          this.isSearching = false;
          self.handleData(this.cacheData);
        }
      },
      clearSearching: function () {
        this.searchKey = "";
        this.status = "";
        this.isSearching = false;
        self.handleData(this.cacheData);
      },
      dataMapping: function (data) {
        return data.map(function (item) {
          var ob = item;
          ob.createDate = new Date(item.createDate).toDefaultLangString();
          switch (ob.status) {
            case "Paid":
              ob.statusTheme = "green";
              break;
            case "Shipping":
              ob.statusTheme = "orange";
              break;
            default:
              ob.statusTheme = "gray";
              break;
          }
          ob.statusText = Kooboo.text.eCommerce.orderStatus[ob.status] || ob.status;
          ob.Edit = {
            text: Kooboo.text.common.edit,
            url: Kooboo.Route.Get(Kooboo.Route.Order.DetailPage, {
              id: item.id,
            }),
          };
          return ob;
        });
      },
      handleData: function (data) {
        self.pager = data;
        self.tableData = self.dataMapping(data.list);
      },
      cancelOrder: function (data) {
        if (confirm(Kooboo.text.confirm.eCommerce.cancelOrder)) {
          Kooboo.Order.cancel({
            id: data.id,
          }).then(function (res) {
            if (res.success) {
              self.getList(self.currentPageNr);
              window.info.show(
                Kooboo.text.info.eCommerce.cancelOrder.success,
                true
              );
            } else {
              window.info.show(
                Kooboo.text.info.eCommerce.cancelOrder.fail,
                false
              );
            }
          });
        }
      },
      onDelete: function () {
        if (confirm(Kooboo.text.confirm.deleteItems)) {
          var ids = self.selected.map(function (row) {
            return row.id;
          });
          Kooboo.Order.Deletes({
            ids: JSON.stringify(ids),
          }).then(function (res) {
            if (res.success) {
              self.tableData = _.filter(self.tableData, function (row) {
                return ids.indexOf(row.id) === -1;
              });
              self.selected = [];
              window.info.show(Kooboo.text.info.delete.success, true);
            }
          });
        }
      },
      changePage: function (page) {
        self.getList(page);
      },
      getAllLogistics: function () {
        Kooboo.Order.getAllLogistics().then(function (res) {
          if (res.success) {
            self.logistics = res.model;
          }
        });
      },
      ship: function (order) {
        this.currentOrder = order;
        this.shipModel.logisticsCompany = order.logisticsCompany;
        this.shipModel.logisticsNumber = order.logisticsNumber;
        if (!order.customerAddress) {
          this.saveShip();
          return;
        }
        if (!this.logistics || !this.logistics.length) {
          this.getAllLogistics();
        }
        this.visibleShipModal = true;
        setTimeout(function () {
          self.$refs.shipForm.clearValid();
        }, 100);
      },
      getLogisticsNumber: function () {
        if (!self.shipModel.logisticsCompany) {
          return;
        }
        Kooboo.Order.getLogisticsNumber({
          id: this.currentOrder.id,
          logisticsCompany: self.shipModel.logisticsCompany,
        }).then(function (res) {
          if (res.success && res.model) {
            self.shipModel.logisticsNumber = res.model;
          }
        });
      },
      saveShip: function () {
        if (!this.$refs.shipForm.validate()) {
          return;
        }
        if (confirm(Kooboo.text.confirm.eCommerce.shipOrder)) {
          Kooboo.Order.ship({
            orderId: self.currentOrder.id,
            logisticsCompany: self.shipModel.logisticsCompany,
            logisticsNumber: self.shipModel.logisticsNumber,
          }).then(function (res) {
            if (res.success) {
              window.info.show(
                Kooboo.text.info.eCommerce.shipOrder.success,
                true
              );
              self.getList(self.currentPageNr);
              self.visibleShipModal = false;
            } else {
              window.info.show(
                Kooboo.text.info.eCommerce.shipOrder.fail,
                false
              );
            }
          });
        }
      },
      finish: function (order) {
        if (confirm(Kooboo.text.confirm.eCommerce.finishOrder)) {
          Kooboo.Order.finish({
            id: order.id,
          }).then(function (res) {
            if (res.success) {
              window.info.show(
                Kooboo.text.info.eCommerce.finishOrder.success,
                true
              );
              self.getList(self.currentPageNr);
            } else {
              window.info.show(
                Kooboo.text.info.eCommerce.finishOrder.success,
                false
              );
            }
          });
        }
      },
    },
  });
});
