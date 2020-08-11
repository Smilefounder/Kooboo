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
        model: {
          firstName: "",
          lastName: "",
          emailAddress: "",
          telephone: "",
        },
        typesUrl: Kooboo.Route.Customer.ListPage,
        rules: {
          username: [
            { required: Kooboo.text.validation.required },
            {
              min: 5,
              max: 30,
              message:
                Kooboo.text.validation.minLength +
                5 +
                ", " +
                Kooboo.text.validation.maxLength +
                30,
            },
            {
              pattern: /^[a-zA-Z0-9_-]+$/,
              message: Kooboo.text.validation.usernameInvalid,
            },
            {
              remote: {
                url: Kooboo.User.isUniqueName(),
                data: function () {
                  return {
                    name: me.model.username,
                  };
                },
              },
              message: Kooboo.text.validation.taken,
            },
          ],
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
                30,
            },
          ],
          confirmPassword: [
            { required: Kooboo.text.validation.required },
            {
              validate: function (value) {
                return value == me.model.password;
              },
              message: Kooboo.text.validation.notEqual,
            },
          ],
          email: [
            { required: Kooboo.text.validation.required },
            {
              pattern: /^\s*[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+(\.[a-zA-Z0-9!#$%&'*+\-/=?^_`{|}~]+)*@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\s*$/,
              message: Kooboo.text.validation.emailInvalid,
            },
            {
              remote: {
                url: Kooboo.User.isUniqueEmail(),
                data: function () {
                  return {
                    email: me.model.email,
                  };
                },
              },
              message: Kooboo.text.validation.taken,
            },
          ],
        },
      };
    },
    mounted: function () {
      $.when(
        Kooboo.Site.Langs(),
        Kooboo.Customer.getEdit({
          id: self.promotionId,
        })
      ).then(function (r1, r2) {
        var langRes = r1[0];
        customer = r2[0];

        if (langRes.success && customer.success) {
          self.siteLangs = langRes.model;

          var customerViewModel = customer.model.customerViewModel;
          if (customerViewModel) {
            self.model.firstName = customerViewModel.firstName;
            self.model.lastName = customerViewModel.lastName;
            self.model.emailAddress = customerViewModel.emailAddress;
            self.model.telephone = customerViewModel.telephone;
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
        var valid = false;

        valid = true;

        return valid;
      },
    },
    computed: {
      isNew: function () {
        return self.id == Kooboo.Guid.Empty;
      },
    },
  });
});
