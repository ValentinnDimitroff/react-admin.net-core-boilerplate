// import React from 'react'
// import classNames from 'classnames'
// import PropTypes from 'prop-types'
// import { makeStyles } from '@material-ui/styles'
// import { Card, CardHeader, CardContent, CardActions, Divider, Grid, Box } from '@material-ui/core'
// import {
//     EditIcon,
//     FormWithRedirect,
//     required,
//     SaveButton,
//     TextInput,
//     useNotify,
//     useTranslate,
// } from '../../../_design'
// import { httpPost } from '../../../../ra-providers/dataProvider'
// import { apiCustomRoutes } from '../../../../constants'

// const useStyles = makeStyles(() => ({
//     root: {},
//     actions: {
//         justifyContent: 'flex-end',
//     },
// }))

// const Title = () => (
//     <Box display="flex" alignItems="center">
//         <Box mr="10px"> Edit Profile </Box>
//         <EditIcon />
//     </Box>
// )

// const AccountDetails = ({ profileData, className, setProfileData }) => {
//     const notify = useNotify()
//     const classes = useStyles()
//     const translate = useTranslate()

//     const handleSubmit = (values) => {
//         httpPost(apiCustomRoutes.accounts.Profile, values, {
//             onSuccess: (data) => {
//                 setProfileData(data)
//                 notify(translate('account.profile.updated.success'))
//             },
//             onFailure: (error) => {
//                 notify(translate('account.profile.updated.failure'), 'warning')
//             },
//         })
//     }

//     return (
//         <Card className={classNames(classes.root, className)}>
//             <CardHeader
//                 // subheader="The information can be edited"
//                 title={<Title />}
//             />
//             <Divider />
//             <FormWithRedirect
//                 initialValues={profileData}
//                 redirect={false}
//                 save={handleSubmit}
//                 onSubmit={handleSubmit}
//                 //validateOnBlur
//                 variant="outlined"
//                 render={(formProps) => {
//                     return (
//                         <form>
//                             {/* autoComplete="off" */}
//                             <CardContent>
//                                 <Grid container spacing={3}>
//                                     <Grid item md={6} xs={12}>
//                                         <TextInput
//                                             label="First name"
//                                             source="firstName"
//                                             resource="profile"
//                                             fullWidth
//                                             margin="dense"
//                                             helperText="Please specify the first name"
//                                             validate={[required()]}
//                                             variant={formProps.variant}
//                                         />
//                                     </Grid>
//                                     <Grid item md={6} xs={12}>
//                                         <TextInput
//                                             label="Last name"
//                                             source="lastName"
//                                             fullWidth
//                                             margin="dense"
//                                             validate={[required()]}
//                                             variant={formProps.variant}
//                                         />
//                                     </Grid>
//                                     <Grid item md={6} xs={12}>
//                                         <TextInput
//                                             label="Email Address"
//                                             source="email"
//                                             fullWidth
//                                             disabled
//                                             margin="dense"
//                                             validate={[required()]}
//                                             variant={formProps.variant}
//                                         />
//                                     </Grid>
//                                     <Grid item md={6} xs={12}>
//                                         <TextInput
//                                             label="Phone Number"
//                                             source="phoneNumber"
//                                             fullWidth
//                                             margin="dense"
//                                             variant={formProps.variant}
//                                         />
//                                     </Grid>
//                                 </Grid>
//                             </CardContent>
//                             <Divider />
//                             <CardActions className={classes.actions}>
//                                 <SaveButton
//                                     // {...formProps}
//                                     label="Save details"
//                                     saving={formProps.saving}
//                                     handleSubmitWithRedirect={formProps.handleSubmitWithRedirect}
//                                 />
//                             </CardActions>
//                         </form>
//                     )
//                 }}
//             />
//         </Card>
//     )
// }

// AccountDetails.propTypes = {
//     className: PropTypes.string,
// }

// export default AccountDetails
