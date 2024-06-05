$(function () {
    var l = abp.localization.getResource('CrmApp');
    var createModal = new abp.ModalManager(abp.appPath + 'Products/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Products/EditModal');

    var dataTable = $('#ProductsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(crmApp.products.product.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('CrmApp.Products.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('CrmApp.Products.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('ProductDeletionConfirmationMessage');
                                    },
                                    action: function (data) {
                                        crmApp.products.product
                                            .delete(data.record.id)
                                            .then(function() {
                                                abp.notify.info(
                                                    l('SuccessfullyDeleted')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('ProductCategory'),
                    data: "productCategoryName"
                },
                {
                    title: l('Name'),
                    data: "name",
                },
                {
                    title: l('Description'),
                    data: "description",
                },
                {
                    title: l('Price'),
                    data: "price",
                },
                {
                    title: l('StockQuantity'),
                    data: "stockQuantity",
                },
                {
                    title: l('Photo'),
                    data: "photo",
                },
                {
                    title: l('Notes'),
                    data: "notes",
                },
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewProductButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
