$(function () {
    var l = abp.localization.getResource('CrmApp');
    var createModal = new abp.ModalManager(abp.appPath + 'Vendors/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Vendors/EditModal');

    var dataTable = $('#VendorsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(crmApp.vendors.vendor.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('CrmApp.Vendors.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('CrmApp.Vendors.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('VendorDeletionConfirmationMessage');
                                    },
                                    action: function (data) {
                                        crmApp.vendors.vendor
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
                    title: l('Name'),
                    data: "name",
                },
                {
                    title: l('ContactName'),
                    data: "contactName",
                },
                {
                    title: l('Phone'),
                    data: "phone",
                },
                {
                    title: l('Email'),
                    data: "email",
                },
                {
                    title: l('Address'),
                    data: "addressCity"
                },
                {
                    title: l('Product'),
                    data: "productName"
                },
                {
                    title: l('Service'),
                    data: "serviceName"
                },
                {
                    title: l('Logo'),
                    data: "logo",
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

    $('#NewVendorButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
