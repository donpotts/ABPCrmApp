$(function () {
    var l = abp.localization.getResource('CrmApp');
    var createModal = new abp.ModalManager(abp.appPath + 'Services/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Services/EditModal');

    var dataTable = $('#ServicesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(crmApp.services.service.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('CrmApp.Services.Edit'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('CrmApp.Services.Delete'), //CHECK for the PERMISSION
                                    confirmMessage: function (data) {
                                        return l('ServiceDeletionConfirmationMessage');
                                    },
                                    action: function (data) {
                                        crmApp.services.service
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
                    title: l('ServiceCategory'),
                    data: "serviceCategoryName"
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
                    title: l('Recurring'),
                    data: "recurring",
                },
                {
                    title: l('Icon'),
                    data: "icon",
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

    $('#NewServiceButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
