const ModalComponent = {
    render: (Component, data) => {
        return `<div class="modal fade" id="changePassword-user" tabindex="-1" role="dialog" aria-labelledby="changePasswordLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            ${Component.render(data)}
                        </div>
                    </div>
                </div>`
    }
}