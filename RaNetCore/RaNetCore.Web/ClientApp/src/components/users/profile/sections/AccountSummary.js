// import React, { useState } from 'react'
// import PropTypes from 'prop-types'
// import classNames from 'classnames'
// import { makeStyles } from '@material-ui/styles'
// import {
//     Card,
//     CardActions,
//     CardContent,
//     Typography,
//     Divider,
//     LinearProgress,
// } from '@material-ui/core'
// import { CloudUploadIcon, UploadPictureButton, useNotify, useTranslate } from '../../../_design'
// import { convertFileToBase64 } from '../../../../ra-providers/dataProvider-utils/images-handler'
// import { httpClient } from '../../../../ra-providers/dataProvider'
// import { apiCustomRoutes } from '../../../../constants'
// import UserProfileAvatar from '../../common/UserProfileAvatar'

// const useStyles = makeStyles((theme) => ({
//     root: {},
//     details: {
//         display: 'flex',
//         alignItems: 'center',
//         flexDirection: 'column',
//     },
//     progress: {
//         marginTop: theme.spacing(2),
//     },
//     uploadButtonLabel: {
//         margin: '0px 8px',
//         width: '100%',
//     },
// }))

// const calcCompleteness = ({ firstName, lastName, email, phoneNumber, picture }) => {
//     const filledFields = !!firstName + !!lastName + !!email + !!phoneNumber + !!picture

//     return Math.trunc((filledFields / 5) * 10) * 10
// }

// const AccountSummary = ({ className, profileData, ...rest }) => {
//     const fullName = profileData && `${profileData.firstName} ${profileData.lastName}`
//     const completePercentage = calcCompleteness(profileData)
//     const classes = useStyles()
//     const notify = useNotify()
//     const translate = useTranslate()
//     const [picture, setPicture] = useState(profileData.picture)

//     const onPictureUpload = (file) => {
//         convertFileToBase64({ rawFile: file })
//             .then((fileBase64) => {
//                 return httpClient(apiCustomRoutes.accounts.UploadPicture, {
//                     method: 'POST',
//                     body: JSON.stringify({ base64Image: fileBase64 }),
//                 })
//             })
//             .then((response) => {
//                 if (response.status !== 200) throw response

//                 return response.json
//             })
//             .then((data) => {
//                 setPicture(data.picture)
//                 notify(translate('account.profile.pictureUpload.success'))
//             })
//             .catch((error) => {
//                 notify(translate('account.profile.pictureUpload.failure'), 'warning')
//             })
//     }

//     return (
//         <Card {...rest} className={classNames(classes.root, className)}>
//             <CardContent>
//                 <div className={classes.details}>
//                     <UserProfileAvatar record={{ fullName, picture }} />
//                     <Typography gutterBottom variant="h3">
//                         {fullName}
//                     </Typography>
//                     <Typography color="textSecondary" variant="body1">
//                         {`Sofia, Bulgaria`}
//                     </Typography>
//                     <Typography color="textSecondary" variant="body1">
//                         {/* {moment().format('hh:mm A')} ({user.timezone}) */}
//                     </Typography>
//                 </div>
//                 <div className={classes.progress}>
//                     <Typography variant="body1">
//                         {`Profile Completeness: ${completePercentage}%`}
//                     </Typography>
//                     <LinearProgress value={completePercentage} variant="determinate" />
//                 </div>
//             </CardContent>
//             <Divider />
//             <CardActions>
//                 <UploadPictureButton
//                     icon={<CloudUploadIcon />}
//                     className={classes.uploadButtonLabel}
//                     onUpload={onPictureUpload}
//                 />
//             </CardActions>
//         </Card>
//     )
// }

// AccountSummary.propTypes = {
//     className: PropTypes.string,
// }

// export default AccountSummary
