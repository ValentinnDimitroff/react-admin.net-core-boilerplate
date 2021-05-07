import React from 'react'
import PropTypes from 'prop-types'
import Grid from '@material-ui/core/Grid'
import Divider from '@material-ui/core/Divider'
import CardContent from '@material-ui/core/CardContent'
import CardActions from '@material-ui/core/CardActions'
import {
    FormWithRedirect,
    required,
    SaveButton,
    TextInput,
    useNotify,
    useTranslate,
} from 'react-admin'
import { apiCustomRoutes } from '../../../../constants'
import { httpPost } from '../../../../ra-providers/dataProvider'
import AccountDetailsSection from '../../common/AccountDetailsSection'
import AccountSectionForm from '../../common/AccountSectionForm'

const AccountDetails = ({ profileData, setProfileData }) => {
    const notify = useNotify()
    const translate = useTranslate()

    const handleSubmit = (values) => {
        httpPost(apiCustomRoutes.accounts.Profile, values, {
            onSuccess: (data) => {
                // TODO echange with dispatch
                setProfileData(data)
                notify(translate('account.profile.updated.success'))
            },
            onFailure: (error) => {
                notify(translate('account.profile.updated.failure'), 'warning')
            },
        })
    }

    return (
        <AccountDetailsSection title="Edit Profile">
            <AccountSectionForm onSubmit={handleSubmit} submitBtnText="Save details">
                {
                    (formProps) => (
                        <Grid container spacing={3}>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="First name"
                                    source="firstName"
                                    resource="profile"
                                    fullWidth
                                    margin="dense"
                                    helperText="Please specify the first name"
                                    validate={[required()]}
                                    variant={formProps.variant}
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="Last name"
                                    source="lastName"
                                    fullWidth
                                    margin="dense"
                                    validate={[required()]}
                                    variant={formProps.variant}
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="Email Address"
                                    source="email"
                                    fullWidth
                                    disabled
                                    margin="dense"
                                    validate={[required()]}
                                    variant={formProps.variant}
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextInput
                                    label="Phone Number"
                                    source="phoneNumber"
                                    fullWidth
                                    margin="dense"
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

AccountDetails.propTypes = {
    className: PropTypes.string,
}

export default AccountDetails
