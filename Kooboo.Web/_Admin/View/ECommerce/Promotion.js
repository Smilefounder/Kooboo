$(function () {
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        promotionId: Kooboo.getQueryString("id") || Kooboo.Guid.Empty,
        startValidating: false,
        validationPassed: false,
        siteLangs: {},
        categories: [],
        selectedCategories: [],
        multipleMedia: false,
        mediaDialogData: {},
        typesUrl: Kooboo.Route.Promotion.ListPage,
        showCategoriesModal: false,
        model: {
          name: "",
          isActive: false,
          canCombine: false,
          activeBasedOnDates: false,
          startDate: moment().format("YYYY-MM-DD HH:mm"),
          endDate: moment().format("YYYY-MM-DD HH:mm"),
          ruleType: "",
          promotionMethod: "",
          // -1 不让传给后台
          promotionTarget: -1,
          priceAmountReached: "",
          amount: 0,
          percent: 0,
        },
        ruleTypes: [],
        promotionMethods: [],
        promotionTargets: [],
      };
    },
    mounted: function () {
      $.when(
        Kooboo.Site.Langs(),
        Kooboo.PromotionRule.getEdit({
          id: self.promotionId,
        }),
        Kooboo.ProductCategory.getList()
      ).then(function (r1, r2, r3) {
        var langRes = r1[0];
        promotionRules = r2[0];
        cateRes = r3[0];

        if (langRes.success && promotionRules.success && cateRes.success) {
          self.siteLangs = langRes.model;
          self.ruleTypes = promotionRules.model.promotionRuleTypes || [];
          self.promotionMethods = promotionRules.model.promotionMethods || [];
          self.promotionTargets = promotionRules.model.promotionTargets || [];

          var promotionViewModel = promotionRules.model.promotionViewModel;
          if (promotionViewModel) {
            self.model.name = promotionViewModel.name;
            self.model.isActive = promotionViewModel.isActive;
            self.model.canCombine = promotionViewModel.canCombine;
            self.model.activeBasedOnDates =
              promotionViewModel.activeBasedOnDates;
            self.model.startDate = promotionViewModel.startDate;
            self.model.endDate = promotionViewModel.endDate;
            self.model.ruleType = promotionViewModel.ruleType;
            self.model.promotionMethod = promotionViewModel.promotionMethod;
            self.model.promotionTarget = promotionViewModel.forObject;
            self.model.priceAmountReached =
              promotionViewModel.priceAmountReached;
            self.model.amount = promotionViewModel.amount;
            self.model.percent = promotionViewModel.percent;

            self.categories = self.getCategories(
              cateRes.model,
              promotionViewModel.categories || []
            );
            self.selectedCategories = getSelected(self.categories);
          }
        }
      });
    },
    methods: {
      onSaveAndReturn: function () {
        self.onSave(function () {
          location.href = Kooboo.Route.Promotion.ListPage;
        });
      },
      onSave: function (cb) {
        if (self.isValid()) {
          var categories = self.selectedCategories.map(function (cate) {
            return cate.id;
          });
          Kooboo.PromotionRule.post({
            id: self.promotionId,
            promotionModel: self.model,
            categories: categories,
          }).then(function (res) {
            if (res.success) {
              location.href = Kooboo.Route.Promotion.ListPage;
              // console.log(cb)
              // console.log(typeof cb)
              // if (cb && typeof cb == "function") {
              //   cb();
              // } else {
              //   location.href = Kooboo.Route.Get(
              //     Kooboo.Route.Promotion.DetailPage,
              //     {
              //       id: res.model,
              //       type: typeId,
              //     }
              //   );
              // }
            }
          });
        }
      },
      isValid: function () {
        // var valid = self.$refs.fieldPanel.validate();
        // if (!valid) return;

        // self.typesMatrix.forEach(function (row) {
        //   if (
        //     self.validateInput(row, "price") ||
        //     self.validateInput(row, "stock")
        //   ) {
        //     valid = false;
        //   }
        // });
        var valid = true;

        return valid;
      },
      getCategories: function (cates, selectedIds) {
        var temp = [];
        cates.forEach(function (c) {
          c.selected = selectedIds.indexOf(c.id) > -1;
          if (c.subCats && c.subCats.length) {
            c.subCats = self.getCategories(c.subCats, selectedIds);
          }
          temp.push(c);
        });

        return temp;
      },
      onShowCategoriesModal: function () {
        self.showCategoriesModal = true;
      },
      onHideCategoriesModal: function () {
        self.showCategoriesModal = false;
      },
      onSaveCategoriesModal: function () {
        self.selectedCategories = getSelected(self.categories);
        self.onHideCategoriesModal();
      },
      validateInput: function (data, fieldName) {
        return (data.error[fieldName] = data[fieldName] === "");
      },
    },
    computed: {
      isNew: function () {
        return self.promotionId == Kooboo.Guid.Empty;
      },
    },
    watch: {
      "model.promotionMethod": function (newValue, oldValue) {
        if (newValue && newValue === "Amount") {
          self.model.percent = 0;
        }
        if (newValue && newValue === "Percent") {
          self.model.amount = 0;
        }
      },
      "model.ruleType": function (newValue, oldValue) {
        if (newValue && newValue === "ByTotalAmount") {
          self.selectedCategories = [];
        }
        if (newValue && newValue === "ByProductCategory") {
          self.model.priceAmountReached = 0;
        }
      },
    },
  });

  function getSelected(cates) {
    var temp = [];
    cates.forEach(function (c) {
      if (c.selected) {
        temp.push(c);
      }

      if (c.subCats && c.subCats.length) {
        temp = _.concat(temp, getSelected(c.subCats));
      }
    });
    return temp;
  }

  Vue.component("product-category", {
    template: "#category-template",
    props: {
      category: Object,
    },
  });
});
