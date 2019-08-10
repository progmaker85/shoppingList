(function () {
    'use strict';

    angular.module("ShoppingListCheckOffApp", [])
        .controller("ShoppingListController", shoppingListController)
        .service("ShoppingListCheckOffService", shoppingListCheckOffService)
        //.constant('ApiBasePath', "http://localhost:5000/api/shoppinglist");
        .constant('ApiBasePath', "http://shoppinglistapi-dev.eu-central-1.elasticbeanstalk.com/api/shoppinglist");


    shoppingListController.$inject = ['$q', 'ShoppingListCheckOffService'];
    function shoppingListController($q, shoppingListService) {
        var list = this;

        list.shoppingItems = [];
        list.boughtItems = [];

        list.addShoppingItem = function (itemName, itemQuantity) {
            shoppingListService.addItem(itemName, itemQuantity)
                .then(function () {
                    list.updateItemsList();
                })
                .catch(function (error) {
                    console.log("Something went terribly wrong." + error);
                });
        };

        list.updateItemsList = function () {
            var shoppingItemsPromise = shoppingListService.getShopingItems().then(function (result) {
                list.shoppingItems = result;
            });
            var boughtItemsPromise = shoppingListService.getBoughtItems().then(function (result) {
                list.boughtItems = result;
            });
            $q.all([shoppingItemsPromise, boughtItemsPromise]).catch(function (error) {
                console.log("Something went terribly wrong." + error);
            });
        };

        list.checkOutItem = function (itemIndex) {
            shoppingListService.checkOutItem(list.shoppingItems[itemIndex].id).then(function () {
                list.updateItemsList();
            }).catch(function (error) {
                console.log("Something went terribly wrong." + error);
            });
        }

        list.updateItemsList();
    };

    shoppingListCheckOffService.$inject = ['$http', 'ApiBasePath'];
    function shoppingListCheckOffService($http, ApiBasePath) {
        var service = this;

        service.addItem = function (itemName, itemQuantity) {
            return $http({
                method: "POST",
                url: (ApiBasePath + "/add"),
                data: JSON.stringify({ ItemName: itemName, Quantity: itemQuantity })
            });
        };
        service.checkOutItem = function (itemIndex) {
            return $http({
                method: "PUT",
                url: (ApiBasePath + "/checkOff"),
                params: { "id": itemIndex }
            });
        };
        service.getBoughtItems = function () {
            return $http({
                method: "GET",
                url: (ApiBasePath + "/boughtItems")
            }).then(function (result) {
                // return processed items
                return result.data;
            });
        };
        service.getShopingItems = function () {
            return $http({
                method: "GET",
                url: (ApiBasePath)
            }).then(function (result) {
                // return processed items
                return result.data;
            });
        };
    };
})();
