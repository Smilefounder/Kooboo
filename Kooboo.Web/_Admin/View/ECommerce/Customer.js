$(function () {
  var CONTENT_ID = Kooboo.getQueryString("id");
  var self;
  new Vue({
    el: "#main",
    data: function () {
      self = this;
      return {
        id: CONTENT_ID || Kooboo.Guid.Empty,
        siteLangs: null,
        editOriginEmailAddress: "",
        editOriginUserName: "",
        model: {
          userName: "",
          firstName: "",
          lastName: "",
          emailAddress: "",
          telephone: "",
          password: "",
        },
        typesUrl: Kooboo.Route.Customer.ListPage,
      };
    },
    mounted: function () {
      $.when(
        Kooboo.Site.Langs(),
        Kooboo.Customer.getEdit({
          id: self.id,
        })
      ).then(function (r1, r2) {
        var langRes = r1[0];
        customer = r2[0];

        if (langRes.success && customer.success) {
          self.siteLangs = langRes.model;

          var customerViewModel = customer.model.customerViewModel;
          if (customerViewModel) {
            self.model.userName = customerViewModel.userName;
            self.model.firstName = customerViewModel.firstName;
            self.model.lastName = customerViewModel.lastName;
            self.model.emailAddress = customerViewModel.emailAddress;
            self.model.telephone = customerViewModel.telephone;

            self.editOriginEmailAddress = customerViewModel.emailAddress;
            self.editOriginUserName = customerViewModel.userName;
          }
        }
      });
    },
    methods: {
      onSaveAndReturn: function () {
        self.onSave(function () {
          location.href = Kooboo.Route.Customer.ListPage;
        });
      },
      onSave: function (cb) {
        if (self.isValid()) {
          Kooboo.Customer.post({
            id: self.id,
            customerModel: self.model,
          }).then(function (res) {
            if (res.success) {
              if (cb && typeof cb == "function") {
                cb();
              } else {
                location.href = Kooboo.Route.Get(
                  Kooboo.Route.Customer.DetailPage,
                  {
                    id: res.model,
                  }
                );
              }
            }
          });
        }
      },

      isValid: function () {
        return this.$refs.form.validate();
      },
    },
    computed: {
      isNew: function () {
        return self.id == Kooboo.Guid.Empty;
      },
      rules: function () {
        var rules = {
          userName: [{ required: Kooboo.text.validation.required }],
          password: [
            { required: Kooboo.text.validation.required },
            {
              min: 1,
              max: 30,
              message:
                Kooboo.text.validation.minLength +
                1 +
                ", " +
                Kooboo.text.validation.maxLength +
                30
            }
          ],
          firstName: [{ required: Kooboo.text.validation.required }],
          lastName: [{ required: Kooboo.text.validation.required }],
          emailAddress: [
            { required: Kooboo.text.validation.required },
            {
              pattern: /^\s*[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+(\.[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+)*@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\s*$/,
              message: Kooboo.text.validation.emailInvalid,
            },
          ],
          telephone: [{ required: Kooboo.text.validation.required }],
        };

        if (
          this.isNew ||
          self.editOriginEmailAddress != self.model.emailAddress
        ) {
          rules.emailAddress.push({
            remote: {
              url: Kooboo.Customer.isUniqueEmail(),
              data: function () {
                return {
                  email: self.model.emailAddress,
                };
              },
            },
            message: Kooboo.text.validation.taken,
          });
        }

        if (this.isNew || self.editOriginUserName != self.model.userName) {
          rules.userName.push({
            remote: {
              url: Kooboo.Customer.isUniqueName(),
              data: function () {
                return {
                  userName: self.model.userName,
                };
              },
            },
            message: Kooboo.text.validation.taken,
          });
        }
        return rules;
      },
    },
  });
});
