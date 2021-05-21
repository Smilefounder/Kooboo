(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/kbForm.js",
  ]);

  Vue.component("kb-authorize-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/openApi/kb-authorize-modal.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        model: null,
        data: {},
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
      securities() {
        var result = [];

        if (this.model) {
          var doc = JSON.parse(this.model.jsonData);

          if (doc.components && doc.components.securitySchemes) {
            for (const key in doc.components.securitySchemes) {
              result.push({
                name: key,
                type: doc.components.securitySchemes[key].type,
                value: doc.components.securitySchemes[key],
              });
            }
          }
        }

        return result;
      },
    },
    methods: {
      save() {
        var copyed = JSON.parse(JSON.stringify(this.model));
        copyed.securities = this.data;
        Kooboo.OpenApi.post(copyed).then((rsp) => {
          if (rsp.success) {
            this.show = false;
            this.$emit("ok");
          }
        });
      },
      getScheme(item) {
        if (!item.scheme) return null;
        return item.scheme.toLowerCase();
      },
      getData(key) {
        if (!this.data[key]) {
          var data;

          for (const i in this.model.securities) {
            if (key.toLocaleLowerCase() == i.toLocaleLowerCase()) {
              data = this.model.securities[i];
            }
          }

          if (!data) {
            data = {
              username: "",
              password: "",
              token: "",
            };
          }

          Vue.set(this.data, key, data);
        }

        return this.data[key];
      },
    },
    watch: {
      value(value) {
        if (value) {
          Kooboo.OpenApi.get({ id: this.id }).then((rsp) => {
            if (rsp.success) {
              this.model = rsp.model;
            }
          });
        } else this.model = null;
      },
    },
  });
})();
