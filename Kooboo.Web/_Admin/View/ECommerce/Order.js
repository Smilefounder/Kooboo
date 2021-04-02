$(function () {
    new Vue({
      el: "#app",
      data: function () {
        return {
          breads: [
            {
              name: "SITES",
            },
            {
              name: "DASHBOARD",
            },
            {
              name: Kooboo.text.common.ProductCategories,
            },
          ],
          list: null,
        };
      },
      mounted() {
      },
      methods: {
      },
    });
  });
  