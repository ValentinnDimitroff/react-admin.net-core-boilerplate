import React from 'react'
import PropTypes from 'prop-types'
import Grid from '@material-ui/core/Grid'
import {
    required,
    TextInput,
    useNotify,
    useTranslate,
} from 'react-admin'
import { apiCustomRoutes } from '../../../../constants'
import { httpPost } from '../../../../ra-providers/dataProvider'
import AccountDetailsSection from '../../common/AccountDetailsSection'
import AccountSectionForm from '../../common/AccountSectionForm'

const AccountSecurity = () => {
    const notify = useNotify()
    const translate = useTranslate()

    const handleSubmit = (values) => {
        httpPost(apiCustomRoutes.accounts.ChangePassword, values, {
            onSuccess: (data) => {
                // TODO echange with dispatch
                notify(translate('account.profile.updated.success'))
            },
            onFailure: (error) => {
                notify(translate('account.profile.updated.failure'), 'warning')
            },
        })
    }

    return (
        <AccountDetailsSection title="Change Password" >
            <AccountSectionForm
                onSubmit={handleSubmit}
                submitBtnText="Update Password"
            >
                {
                    (formProps) => (
                        <Grid container spacing={3}>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="Current Password"
                                    source="currentPassword"
                                    fullWidth
                                    type="password"
                                    margin="dense"
                                    validate={[required()]}
                                    variant={formProps.variant}
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="New Password"
                                    source="newPassword"
                                    fullWidth
                                    type="password"
                                    margin="dense"
                                    validate={[required()]}
                                    variant={formProps.variant}
                                />
                            </Grid>
                        </Grid>
                    )
                }
            </AccountSectionForm>
        </AccountDetailsSection>
    )
}

AccountSecurity.propTypes = {

}

export default AccountSecurity
