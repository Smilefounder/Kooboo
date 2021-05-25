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
        siteId: Kooboo.getQueryString("SiteId"),
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
                name: this.standardName(key),
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
      save(callback) {
        var copyed = JSON.parse(JSON.stringify(this.model));
        copyed.securities = this.data;
        Kooboo.OpenApi.post(copyed).then((rsp) => {
          if (rsp.success) {
            this.show = false;
            this.$emit("ok");
            if (callback.call) callback();
          }
        });
      },
      getScheme(item) {
        if (!item.scheme) return null;
        return item.scheme.toLowerCase();
      },
      getFlow(item) {
        var result = null;
        if (!item.flows) return result;

        for (const key in item.flows) {
          result = {
            name: key,
            flow: item.flows[key],
          };
        }

        return result;
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
              scope: "",
            };
          }
          
          data.redirectUrl = this.getRedirectUrl(key);
          Vue.set(this.data, key, data);
        }

        return this.data[key];
      },
      getTypeDisplay(item) {
        var result = item.type;
        var scheme = this.getScheme(item.value);
        var flow = this.getFlow(item.value);
        if (item.value.in) result += ` [In:${item.value.in}] `;
        if (item.value.name) result += ` [Name:${item.value.name}] `;
        if (scheme) result += ` ,${scheme}`;
        if (flow) result += ` ,${flow.name}`;
        return result;
      },
      getRedirectUrl(name) {
        return `${location.origin}/_api/openapioauth2callback/${this.siteId}/${this.id}/${name}`;
      },
      challenge(item) {
        this.save(() => {
          var flow = this.getFlow(item.value).flow;
          var data = this.getData(item.name);
          var redirectUrl = this.getRedirectUrl(item.name, true);
          var url = `${
            flow.authorizationUrl
          }?response_type=code&state=${new Date().getTime()}&client_id=${
            data.clientId
          }&redirect_uri=${redirectUrl}`;

          if (flow.scopes) {
            var scopes = [];
            for (const key in flow.scopes) scopes.push(key);
            url += `&scope=${encodeURI(scopes.join(" "))}`;
          }

          var win = window.open(url, "win");
        });
      },
      standardName(s) {
        return s.replace(/[^\w]/g, "_").toLowerCase();
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
