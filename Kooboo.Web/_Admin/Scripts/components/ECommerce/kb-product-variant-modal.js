(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/kbTable.js",
    "/_Admin/Scripts/components/kbPager.js",
  ]);

  Vue.component("kb-product-variant-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-product-variant-modal.html"
    ),
    props: ["value"],
    data() {
      return {
        pager: {
          pageNr: 1,
          totalPages: 1,
        },
        list: [],
        pageSize: 10,
        selected: null,
      };
    },
    computed: {
      show: {
        get: function () {
          return this.value;
        },
        set: function (value) {
          this.$emit("input", value);
        },
      },
    },
    methods: {
      save() {
        this.$emit("ok", this.selected);
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
      disabledCheckbox(row, item) {
        if (!row.enable) return true;
        if (!item.enable) return true;
        if (item.stock <= 0) return true;
        return false;
      },
      changeSelected(row, item) {
        if (this.disabledCheckbox(row, item)) return;
        if (this.selected == item) this.selected = null;
        else this.selected = item;
      },
    },
    watch: {
      value(value) {
        if (!value) return;
        this.changePage(1);
      },
    },
  });
})();
