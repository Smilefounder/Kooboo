(function () {
  Kooboo.loadJS(["/_Admin/Scripts/components/kbTable.js"]);

  Vue.component("kb-selector-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/ECommerce/kb-selector-modal.html"
    ),
    props: ["value", "list", "multiSelect", "showHeader"],
    data() {
      return {
        selectedRows: [],
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
      cols() {
        if (!this.list || !this.list.length) return [];
        var result = [];
        for (const key in this.list[0]) {
          if (["key", "id"].includes(key)) continue;
          result.push(key);
        }

        return result;
      },
    },
    methods: {
      save() {
        this.$emit("ok", this.selectedRows);
        this.selectedRows = [];
        this.show = false;
      },
    },
  });
})();
