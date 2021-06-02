(function () {
  Kooboo.loadJS([
    "/_Admin/Scripts/kooboo/Guid.js",
    "/_Admin/Scripts/components/kbForm.js",
  ]);

  Vue.component("kb-openapi-modal", {
    template: Kooboo.getTemplate(
      "/_Admin/Scripts/components/openApi/kb-openapi-modal.html"
    ),
    props: ["value", "id"],
    data() {
      return {
        model: null,
        file: null,
        accept: [
          "application/json",
          "application/x-yaml",
          "text/json",
          "text/yaml",
          "text/yaml",
          "text/x-yaml",
        ],
        templates: [],
        templateBaseUrl: "http://openapi_template.kooboo.net",
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
      isEdit() {
        return !!this.id;
      },
      modelRules() {
        var me = this;

        var result = {
          name: [
            { required: Kooboo.text.validation.required },
            {
              pattern: /^([A-Za-z][\w\-\.]*)*[A-Za-z0-9]$/,
              message: Kooboo.text.validation.objectNameRegex,
            },
            {
              min: 2,
              max: 24,
              message:
                Kooboo.text.validation.minLength +
                2 +
                ", " +
                Kooboo.text.validation.maxLength +
                24,
            },
          ],
        };

        if (!this.id) {
          result.name.push({
            remote: {
              url: Kooboo.OpenApi.isUniqueName(),
              data() {
                return {
                  name: me.model.name,
                };
              },
            },
            message: Kooboo.text.validation.taken,
          });
        }

        if (this.model.type == "url") {
          result.url = [
            { required: Kooboo.text.validation.required },
            {
              pattern:
                /[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/,
              message: Kooboo.text.validation.urlInvalid,
            },
          ];
        }

        return result;
      },
      selectedTemplate() {
        if (!this.model) return null;
        return this.templates.find(
          (f) => this.getTemplateUrl(f._id) == this.model.url
        );
      },
    },
    methods: {
      save(callback) {
        if (!this.$refs.form.validate()) return;

        Kooboo.OpenApi.post(this.model).then((rsp) => {
          if (rsp.success) {
            this.show = false;
            this.$emit("ok");
            if (callback.call) callback();
          }
        });
      },
      selectedFile(files) {
        if (files.length) {
          this.file = files[0];
          var reader = new FileReader();

          reader.onload = (rsp) => {
            this.model.jsonData = rsp.target.result;
          };

          reader.readAsText(files[0]);
        }
      },
      addCache() {
        this.model.caches.push({
          method: "Get",
          pattern: "",
          expiresIn: 0,
        });
      },
      selectTemplate(item) {
        this.model.url = this.getTemplateUrl(item._id);
        this.model.baseUrl = item.baseUrl;
        this.model.name = item.name;
      },
      getTemplateUrl(id) {
        return `${this.templateBaseUrl}/detail?id=${id}`;
      },
    },
    watch: {
      value(value) {
        if (value) {
          if (this.id) {
            Kooboo.OpenApi.get({ id: this.id }).then((rsp) => {
              if (rsp.success) {
                this.model = rsp.model;
              }
            });
          } else {
            this.model = {
              name: "",
              type: "url",
              url: "",
              jsonData: "",
              caches: [],
              templateId: null,
            };
          }

          fetch(`${this.templateBaseUrl}/list`, {
            method: "get",
          }).then(async (rsp) => {
            this.templates = await rsp.json();
          });
        } else {
          this.model = null;
        }
      },
    },
  });
})();
