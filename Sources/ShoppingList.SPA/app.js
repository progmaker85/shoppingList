(function () {
    'use strict';

    angular.module("ShoppingListCheckOffApp", [])
        .controller("ShoppingListController", shoppingListController)
        .service("ShoppingListCheckOffService", shoppingListCheckOffService)
        //.constant('ApiBasePath', "http://localhost:5000/api/shoppinglist");
        .constant('ApiBasePath', "https://shoppinglistapi.progmaker85.name/api/shoppinglist");


    shoppingListController.$inject = ['$q', 'ShoppingListCheckOffService', '$interval'];
    function shoppingListController($q, shoppingListService, $interval) {
        var list = this;

        list.itemName = null;
        list.itemQuantity = null;
        list.shoppingItems = [];
        list.boughtItems = [];
        var updateLocked = false;

        $interval(function () {
            if (updateLocked) return;
            list.updateItemsList();
        }, 30000);

        list.addShoppingItem = function (itemName, itemQuantity) {
            updateLocked = true;
            list.shoppingItems.push({ itemName: itemName, quantity: itemQuantity });
            shoppingListService.addItem(itemName, itemQuantity).finally(function () {
                updateLocked = false;
            }).catch(function (error) {
                console.log("Something went terribly wrong." + error);
            });
            list.itemName = null;
            list.itemQuantity = null;
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
            updateLocked = true;
            shoppingListService.checkOutItem(list.shoppingItems[itemIndex].id)
                .finally(function () {
                    updateLocked = false
                })
                .catch(function (error) {
                    console.log("Something went terribly wrong." + error);
                });

            var removedItem = list.shoppingItems.splice(itemIndex, 1);
            list.boughtItems.push(removedItem[0]);
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
