describe("carController", function () {
    var _scope, _controllerFactory, _httpBackend, _q;

    beforeEach(function () {
        module("testHabanero");
        module("testHabanero.controllers");
    });

    beforeEach(inject(function ($controller, $rootScope, $httpBackend, $q) {
        _scope = $rootScope.$new();
        _controllerFactory = $controller;
        _httpBackend = $httpBackend;
        _q = $q;
    }));

    var createController = function () {
        var ctrl = _controllerFactory("carController",
        { $scope: _scope, $httpBackend: _httpBackend, $q: _q });
        return ctrl;
    };

    describe("init", function () {
        it("should set cars to empty array", function () {
            //arrange 
            var expected = [];
            var controller = createController();
            //act 
            controller.init();
            var actual = controller.cars;
            //assert
            expect(actual).toEqual(expected);
        });
    });
});